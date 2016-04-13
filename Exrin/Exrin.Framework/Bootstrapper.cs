using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class Bootstrapper
    {

        protected readonly AsyncLock _lock = new AsyncLock();
        protected readonly IInjection _injection;
        private readonly Action<object> _setRoot;
        private readonly IList<Action> _postRun = new List<Action>();

        public Bootstrapper(IInjection injection, Action<object> setRoot)
        {
            _injection = injection;
            _setRoot = setRoot;
        }

        public IInjection Init()
        {

            _injection.Init();

            InitServices();

            InitRunners();

            InitStacks();

            InitModels();

            _injection.Complete();

            foreach (var action in _postRun)
                action();

            return _injection;

        }

        /// <summary>
        /// Will initialize the basic navigation and display services
        /// </summary>
        protected virtual void InitServices()
        {
            if (!_injection.IsRegistered<IViewService>())
                _injection.RegisterInterface<IViewService, ViewService>(InstanceType.SingleInstance);

            if (!_injection.IsRegistered<INavigationService>())
                _injection.RegisterInterface<INavigationService, NavigationService>(InstanceType.SingleInstance);

            if (!_injection.IsRegistered<IDisplayService>())
                _injection.RegisterInterface<IDisplayService, DisplayService>(InstanceType.SingleInstance);

            if (!_injection.IsRegistered<IErrorHandlingService>())
                _injection.RegisterInterface<IErrorHandlingService, ErrorHandlingService>(InstanceType.SingleInstance);
        }

        protected virtual void InitStacks()
        {
            MethodInfo method = GetType().GetRuntimeMethod(nameof(RegisterStack), new Type[] { });
            var list = AssemblyHelper.GetTypes(_injection.GetType(), typeof(IStack));

            foreach (var stack in list)
                method.MakeGenericMethod(stack.AsType())
                        .Invoke(this, null);
        }

        protected virtual void InitModels()
        {

            MethodInfo method = _injection.GetType().GetRuntimeMethod(nameof(IInjection.RegisterInterface), new Type[] { typeof(InstanceType) });
            var list = AssemblyHelper.GetTypes(_injection.GetType(), typeof(IBaseModel));

            foreach (var model in list)
            {
                var typeArg = model.ImplementedInterfaces.FirstOrDefault(x => (x.GetTypeInfo().ImplementedInterfaces.Any(y => y == typeof(IBaseModel))));
                if (typeArg != null)
                    method.MakeGenericMethod(typeArg, model.AsType())
                        .Invoke(_injection, new object[] { InstanceType.SingleInstance });
            }

        }

        private void RegisterModel<T>() where T : class, IBaseModel
        {
            _injection.Register<T>(InstanceType.SingleInstance);
        }

        private void InitRunners()
        {
            _injection.RegisterInterface<IStackRunner, StackRunner>(InstanceType.SingleInstance);
            _postRun.Add(() => { _injection.Get<IStackRunner>().Init(_setRoot); });
        }

        public void RegisterStack<T>() where T : class, IStack //TODO: Could be internal instead of public
        {
            _injection.Register<T>(InstanceType.SingleInstance);

            // Register the Stack
            _postRun.Add(() => { _injection.Get<IStackRunner>().RegisterStack<T>(); });

            // Initialize the Stack
            _postRun.Add(() => { _injection.Get<T>().Init(); });
        }




    }
}
