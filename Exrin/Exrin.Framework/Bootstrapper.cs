namespace Exrin.Framework
{
    using Abstraction;
    using Common;
    using Insights;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class Bootstrapper
    {

        protected readonly AsyncLock _lock = new AsyncLock();
        protected readonly IInjection _injection;
        private readonly Action<object> _setRoot;
        protected readonly IList<Action> _postRun = new List<Action>();
        private readonly Dictionary<Type, AssemblyName> _typeAssembly = new Dictionary<Type, AssemblyName>();

        public Bootstrapper(IInjection injection, Action<object> setRoot)
        {
            _injection = injection;
            _setRoot = setRoot;
            _injection.Init();
        }

        public IInjection Init()
        {

            InitCustom();

            InitState();

            InitInsights();

            InitServices();

            InitRunners();

            InitStacks();

            InitModels();

            _injection.Complete();

            foreach (var action in _postRun)
                action();

            PostContainerBuild();

            StartInsights(null);

            return _injection;

        }

        protected virtual void PostContainerBuild() {
            StaticInitialize(new AssemblyName(_injection.GetType().GetTypeInfo().Assembly.GetName().Name));
        }

        protected virtual void InitCustom() { }

        protected virtual void InitState()
        {
            if (!_injection.IsRegistered<INavigationReadOnlyState>())
            {
                var state = new NavigationState();
                _injection.RegisterInstance<INavigationState, NavigationState>(state);
                _injection.RegisterInstance<INavigationReadOnlyState, NavigationState>(state);
            }
        }

        protected virtual void InitInsights()
        {

            if (!_injection.IsRegistered<IInsightStorage>())
                _injection.RegisterInterface<IInsightStorage, MemoryInsightStorage>(InstanceType.SingleInstance);

            if (!_injection.IsRegistered<IDeviceInfo>())
                _injection.RegisterInterface<IDeviceInfo, DeviceInfo>(InstanceType.SingleInstance);

            if (!_injection.IsRegistered<IApplicationInsights>())
                _injection.RegisterInterface<IApplicationInsights, ApplicationInsights>(InstanceType.SingleInstance);

            if (!_injection.IsRegistered<IInsightsProcessor>())
                _injection.RegisterInterface<IInsightsProcessor, Processor>(InstanceType.SingleInstance);
        }

        protected virtual void StartInsights(IList<IInsightsProvider> providers)
        {
            var processor = _injection.Get<IInsightsProcessor>();

            if (providers != null)
                foreach (var provider in providers)
                    processor.RegisterService(provider.ToString(), provider);

            _injection.Get<IInsightsProcessor>().Start();
        }

        /// <summary>
        /// Will initialize the basic navigation and display services and anything implementing the IService interface.
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

            if (!_injection.IsRegistered<IExrinContainer>())
                _injection.RegisterInterface<IExrinContainer, ExrinContainer>(InstanceType.SingleInstance);

            // Register anything with IService implemented
            RegisterBasedOnInterface(typeof(IService));
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
            RegisterBasedOnInterface(typeof(IBaseModel));
        }

        /// <summary>
        /// Goes through the assembly and create a new instance of each IStaticInitialize
        /// to inject static values
        /// </summary>
        /// <param name="name"></param>
        public void StaticInitialize(AssemblyName name)
        {
            foreach (var type in AssemblyHelper.GetTypes(name, typeof(IStaticInitialize)))
            {
                var useConstructor = type.DeclaredConstructors.Where(c => c.CustomAttributes.Any(x => x.AttributeType == typeof(StaticInitialize))).First();

                List<object> parameters = new List<object>();
                foreach (var parameter in useConstructor.GetParameters())
                {
                    parameters.Add(_injection.Get(parameter.ParameterType));
                }

                Activator.CreateInstance(type.AsType(), parameters.ToArray());
            }
       
        }

        public void RegisterTypeAssembly(Type @interface, AssemblyName name)
        {
            if (!_typeAssembly.ContainsKey(@interface))
                _typeAssembly.Add(@interface, name);
        }
        
        // TODO: Improve perf
        private void RegisterBasedOnInterface(Type @interface)
        {
            MethodInfo method = _injection.GetType().GetRuntimeMethod(nameof(IInjection.RegisterInterface), new Type[] { typeof(InstanceType) });
            IList<TypeInfo> list = null;

            if (_typeAssembly.ContainsKey(@interface))
                list = AssemblyHelper.GetTypes(_typeAssembly[@interface], @interface);
            else
                list = AssemblyHelper.GetTypes(_injection.GetType(), @interface);

            foreach (var item in list)
            {
                var typeArg = item.ImplementedInterfaces.FirstOrDefault(x => (x.GetTypeInfo().ImplementedInterfaces.Any(y => y == @interface)));
                if (typeArg != null)
                    method.MakeGenericMethod(typeArg, item.AsType())
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

        public void RegisterStack<T>() where T : class, IStack
        {
            if (!_injection.IsRegistered<T>())
            {
                _injection.Register<T>(InstanceType.SingleInstance);

                // Register the Stack
                _postRun.Add(() => { _injection.Get<IStackRunner>().RegisterStack<T>(); });
            }
            // Initialize the Stack
            _postRun.Add(() => { _injection.Get<T>().Init(); });

        }

    }
}
