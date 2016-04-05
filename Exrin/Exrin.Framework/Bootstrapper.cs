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
        private readonly Action<object> _setPage;
        private readonly IList<Action> _postRun = new List<Action>();

        public Bootstrapper(IInjection injection, Action<object> setPage)
        {
            _injection = injection;
            _setPage = setPage;
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
        private void InitServices()
        {

            _injection.RegisterInterface<IPageService, PageService>(InstanceType.SingleInstance);
            _injection.RegisterInterface<IErrorHandlingService, ErrorHandlingService>(InstanceType.SingleInstance); //TODO: Should be Insights with Error Tracking Capability
            _injection.RegisterInterface<INavigationService, NavigationService>(InstanceType.SingleInstance);
            _injection.RegisterInterface<IDisplayService, DisplayService>(InstanceType.SingleInstance);
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
            _postRun.Add(() => { _injection.Get<IStackRunner>().Init(_setPage); });
        }

        public void RegisterStack<T>() where T : class, IStack
        {
            _injection.Register<T>(InstanceType.SingleInstance);

            // Register the Stack
            _postRun.Add(() => { _injection.Get<IStackRunner>().RegisterStack<T>(); });

            // Initialize the Stack
            _postRun.Add(() => { _injection.Get<T>().Init(); });
        }




    }
}
