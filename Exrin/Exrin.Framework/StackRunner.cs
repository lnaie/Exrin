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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stackChoice"></param>
        /// <param name="args"></param>
        /// <param name="predefinedStack">An ordered array with the Page Keys for a stack to be preloaded. The last page on the array will be the visible one.</param>
        public void Run(object stackChoice, object args = null, Dictionary<string, object> predefinedStack = null)
        {

            if (stackChoice == null)
                throw new NullReferenceException($"{nameof(StackRunner)}.{nameof(Run)} can not accept a null {nameof(stackChoice)}");

            // Don't change to the same stack
            if (_currentStack == stackChoice)
                return;

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
                    await stack.StartNavigation(args: args, loadStartKey: predefinedStack == null); //TODO: check how to wait for in UI Thread

                // Preload Stack
                if (predefinedStack != null)
                    await _navigationService.LoadStack(predefinedStack);

                // Find mainview from ViewHierarchy
                var viewContainer = _viewContainers[_stackViewContainers[stackChoice]];
               
                _setRoot?.Invoke(viewContainer.View);
            });

        }

    }
}
