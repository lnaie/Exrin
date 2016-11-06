namespace Exrin.Framework
{
    using Abstraction;
    using Common;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class NavigationService : INavigationService
    {
        private readonly IViewService _viewService = null;
        private readonly INavigationState _state = null;
        private readonly IDictionary<object, IStack> _stacks = new Dictionary<object, IStack>();
        private readonly IDictionary<object, object> _stackViewContainers = new Dictionary<object, object>();
        private readonly IDictionary<object, IViewContainer> _viewContainers = new Dictionary<object, IViewContainer>();
        private object _currentStack = null;
        private readonly IDisplayService _displayService;
        private readonly IInjectionProxy _injection;
        private Action<object> _setRoot = null;
        private readonly object _lock = new object();

        public NavigationService(IViewService viewService, INavigationState state, IInjectionProxy injection, IDisplayService displayService)
        {
            _viewService = viewService;
            _state = state;
            _displayService = displayService;
            _injection = injection;
        }

        public object ActiveStackIdentifier { get { return _currentStack; } }

        public async Task Navigate(string key)
        {
            await Navigate(key, null); //TODO: Possible locks on these functions, especially since state change
        }

        public async Task Navigate(string viewKey, object args)
        {
            // Navigate on Current Stack
            await _stacks[_currentStack].Navigate(viewKey, args);

            _state.ViewName = viewKey;
        }
        public async Task Navigate<TViewModel>(object args) where TViewModel: class, IViewModel
        {            
            await _stacks[_currentStack].Navigate<TViewModel>(args);
        }

        public async Task Navigate(object stackIdentifier, string viewKey, object args)
        {
            Navigate(new StackOptions() { StackChoice = stackIdentifier });

            await Navigate(viewKey, args);
        }

        public async Task Navigate(string viewKey, object args, IStackOptions options)
        {
            Navigate(options);

            await Navigate(viewKey, args);
        }

        public void Init(Action<object> setRoot)
        {
            _setRoot = setRoot;
        }

        public async Task GoBack()
        {
            await _stacks[_currentStack].GoBack();
        }

        public async Task GoBack(object parameter)
        {
            await _stacks[_currentStack].GoBack(parameter);
        }

        public void RegisterViewContainer<T>() where T : class, IViewContainer
        {
            _injection.Register<T>(InstanceType.SingleInstance);

            var viewContainer = _injection.Get<T>();
            IList<IStack> stacks = new List<IStack>();

            // Load list of stacks depending on ViewContainer
            if (viewContainer as ISingleContainer != null)
            {
                stacks.Add(((ISingleContainer)viewContainer).Stack);
            }
            else if (viewContainer as IMasterDetailContainer != null)
            {
                stacks.Add(((IMasterDetailContainer)viewContainer).MasterStack);
                stacks.Add(((IMasterDetailContainer)viewContainer).DetailStack);
            }
            else if (viewContainer as ITabbedContainer != null)
            {
                foreach (var stack in ((ITabbedContainer)viewContainer).Children)
                    stacks.Add(stack);
            }
            else
            {
                throw new ArgumentException($"{nameof(T)} is not a valid {nameof(IViewContainer)}. Please use one of Exrin's default types");
            }

            // Register stacks and ViewContainer Associations
            foreach (var stack in stacks)
            {
                if (!_stacks.ContainsKey(stack.StackIdentifier))
                    _stacks.Add(stack.StackIdentifier, stack);

                if (!_stackViewContainers.ContainsKey(stack.StackIdentifier))
                    _stackViewContainers.Add(stack.StackIdentifier, viewContainer.Identifier);
            }

            // Register ViewContainers
            if (!_viewContainers.ContainsKey(viewContainer.Identifier))
                _viewContainers.Add(viewContainer.Identifier, viewContainer);
        }

        public StackResult Navigate(IStackOptions options)
        {
            lock (_lock)
            {
                StackResult stackResult = StackResult.StackStarted;

                if (options.StackChoice == null)
                    throw new NullReferenceException($"{nameof(NavigationService)}.{nameof(Navigate)} can not accept a null {nameof(options.StackChoice)}");

                // Don't change to the same stack
                if (_currentStack == options.StackChoice)
                    return StackResult.None;

                if (!_stacks.ContainsKey(options.StackChoice))
                    throw new NullReferenceException($"{nameof(NavigationService)} does not contain a stack named {options.StackChoice.ToString()}");

                // Current / Previous Stack
                IStack oldStack = null;
                if (_currentStack != null)
                {
                    oldStack = _stacks[_currentStack];
                    oldStack.StateChange(StackStatus.Background); // Schedules NoHistoryRemoval
                }

                var stack = _stacks[options.StackChoice];

                _currentStack = options.StackChoice;



                // Set new status
                stack.Proxy.ViewStatus = VisualStatus.Visible;

                // Switch over services
                _displayService.Init(stack.Proxy);

                ThreadHelper.RunOnUIThread(async () =>
                {
                    if (stack.Status == StackStatus.Stopped)
                    {
                        object args = null;

                        // If ArgsKey present only pass args along if the StartKey is the same
                        if ((!string.IsNullOrEmpty(options?.ArgsKey) && stack.NavigationStartKey == options?.ArgsKey) || string.IsNullOrEmpty(options?.ArgsKey))
                        {
                            stackResult = stackResult | StackResult.ArgsPassed;
                            args = options?.Args;
                        }

                        var loadStartKey = options?.PredefinedStack == null;

                        if (loadStartKey)
                            stackResult = stackResult | StackResult.NavigationStarted;

                        await stack.StartNavigation(args: args, loadStartKey: loadStartKey);
                    }

                    //  Preload Stack
                    if (options?.PredefinedStack != null)
                        foreach (var page in options.PredefinedStack)
                            await Navigate(page.Key, page.Value);

                    // Find mainview from ViewHierarchy
                    var viewContainer = _viewContainers[_stackViewContainers[options.StackChoice]];

                    if (viewContainer is IMasterDetailContainer)
                    {
                        var masterDetailContainer = viewContainer as IMasterDetailContainer;
                        if (masterDetailContainer.DetailStack != null)
                        {
                            // Setup Detail Stack
                            var detailStack = _stacks[masterDetailContainer.DetailStack.StackIdentifier];

                            if (detailStack.Status == StackStatus.Stopped)
                                await detailStack.StartNavigation();

                            masterDetailContainer.Proxy.DetailNativeView = detailStack.Proxy.NativeView;

                            // Setup Master Stack
                            var masterStack = _stacks[masterDetailContainer.MasterStack.StackIdentifier];

                            if (masterStack.Status == StackStatus.Stopped)
                                await masterStack.StartNavigation();

                            masterDetailContainer.Proxy.MasterNativeView = masterStack.Proxy.NativeView;
                        }

                    }

                    if (!string.IsNullOrEmpty(options.ViewKey))
                        await Navigate(options.ViewKey, options.Args);

                    _setRoot?.Invoke(viewContainer.NativeView);

                    if (oldStack != null)
                        await oldStack.StackChanged();

                });

                return stackResult;
            }
        }
    }
}