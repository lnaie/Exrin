using Exrin.Abstraction;
using Exrin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
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
            _stacks.Add(stack.StackIdentifier, stack);
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
            _navigationService.Init(stack.Container);
            _displayService.Init(stack.Container);
            
            if (stack.Status == StackStatus.Stopped)
                Task.Run(async () => await stack.StartNavigation(args));

            ThreadHelper.RunOnUIThread(() =>
            {
                _setRoot?.Invoke(stack.Container.View);
            });

        }

    }
}
