namespace Exrin.Framework
{
    using Abstraction;
    using Common;
    using System;
    using System.Collections.Generic;

    public class StackRunner : IStackRunner
    {
        private readonly IDictionary<object, IStack> _stacks = new Dictionary<object, IStack>();
        private readonly IDictionary<object, object> _stackViewContainers = new Dictionary<object, object>();
        private readonly IDictionary<object, IViewContainer> _viewContainers = new Dictionary<object, IViewContainer>();
        private object _currentStack = null;
        private readonly INavigationService _navigationService;
        private readonly IDisplayService _displayService;
        private readonly IInjection _injection;
        private Action<object> _setRoot = null;

        public StackRunner(INavigationService navigationService, IDisplayService displayService, IInjection injection)
        {
            _navigationService = navigationService;
            _displayService = displayService;
            _injection = injection;
        }

        public object CurrentStack
        {
            get
            {
                return _currentStack;
            }
        }

        public void Init(Action<object> setRoot)
        {
            _setRoot = setRoot;
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
                stacks.Add(((IMasterDetailContainer)viewContainer).Master);
                stacks.Add(((IMasterDetailContainer)viewContainer).Detail);
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
        public void Rebuild()
        {
            ThreadHelper.RunOnUIThread(() =>
            {
                var stack = _stacks[_currentStack];
                _setRoot?.Invoke(stack.Container.View);
            });
        }

        public StackResult Run(object stackChoice, IStackOptions options = null)
        {
            StackResult stackResult = StackResult.StackStarted;

            if (stackChoice == null)
                throw new NullReferenceException($"{nameof(StackRunner)}.{nameof(Run)} can not accept a null {nameof(stackChoice)}");

            // Don't change to the same stack
            if (_currentStack == stackChoice)
                return StackResult.None;

            if (!_stacks.ContainsKey(stackChoice))
                throw new NullReferenceException($"{nameof(StackRunner)} does not contain a stack named {stackChoice.ToString()}");

            // Current / Previous Stack
            if (_currentStack != null)
                _stacks[stackChoice].Container.ViewStatus = VisualStatus.Hidden;
          
            var stack = _stacks[stackChoice];

            _currentStack = stackChoice;

            // Set new status
            stack.Container.ViewStatus = VisualStatus.Visible;

            // Switch over services
            _navigationService.Init(stackChoice, stack.Container, stack.ShowNavigationBar);
            _displayService.Init(stack.Container);

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
                        await _navigationService.Navigate(page.Key, page.Value);
                
                // Find mainview from ViewHierarchy
                var viewContainer = _viewContainers[_stackViewContainers[stackChoice]];

                _setRoot?.Invoke(viewContainer.View);

                await _navigationService.StackChanged();

            });

            return stackResult;
        }

    }
}
