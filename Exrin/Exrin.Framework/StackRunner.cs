namespace Exrin.Framework
{
    using Abstraction;
    using Common;
    using System;
    using System.Collections.Generic;

    public class StackRunner : IStackRunner
    {
        private readonly IDictionary<object, IStack> _stacks = new Dictionary<object, IStack>();
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

        public void Init(Action<object> setRoot)
        {
            _setRoot = setRoot;
        }

        public void RegisterStack<T>() where T : class, IStack
        {
            _injection.Register<T>(InstanceType.SingleInstance);

            var stack = _injection.Get<T>();

            if (!_stacks.ContainsKey(stack.StackIdentifier))
                _stacks.Add(stack.StackIdentifier, stack);
        }
        public void Rebuild()
        {
            ThreadHelper.RunOnUIThread(() =>
            {
                var stack = _stacks[_currentStack];
                _setRoot?.Invoke(stack.Container.View);
            });
        }

        public void Run(object stackChoice, object args = null)
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
            
            ThreadHelper.RunOnUIThread(() =>
            {
                if (stack.Status == StackStatus.Stopped)
                    stack.StartNavigation(args); //TODO: check how to wait for in UI Thread

                object mainView;

                // Determines if master view is available
                if (stack.MasterView != null)
                    mainView = stack.MasterView.View;
                else
                    mainView = stack.Container.View;

                _setRoot?.Invoke(mainView);
            });

        }

    }
}
