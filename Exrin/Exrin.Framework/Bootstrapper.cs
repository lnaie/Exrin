using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class Bootstrapper
    {

        protected readonly AsyncLock _lock = new AsyncLock();
        protected readonly IInjection _injection;

        public Bootstrapper(IInjection injection)
        {
            _injection = injection;
        }

        public IInjection Init()
        {

            _injection.Init();

            InitServices();

            InitRunners();

            InitStacks();

            InitModels();

            _injection.Complete();

            return _injection;

        }

        /// <summary>
        /// Will initialize the basic navigation and display services
        /// </summary>
        private void InitServices()
        {

            _injection.Register<IPageService, PageService>();
            _injection.Register<IErrorHandlingService, ErrorHandlingService>(); //TODO: Should be Insights with Error Tracking Capability
            _injection.Register<INavigationService, NavigationService>();
            _injection.Register<IDisplayService, DisplayService>();
        }

        protected virtual void InitStacks() { }

        protected virtual void InitModels() { }

        private void InitRunners()
        {
            _injection.Register<IStackRunner, StackRunner>();
        }

        protected virtual void RegisterStack<T>(object stackChoice) where T : class, IStack
        {
            _injection.Register<T>();
            _injection.Get<IStackRunner>().RegisterStack<T>(stackChoice);
        }
    }
}
