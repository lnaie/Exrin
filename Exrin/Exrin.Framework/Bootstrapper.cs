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
            _postRun.Add(() => { _injection.Get<IStackRunner>().Init(_setPage); });
        }

        protected virtual void RegisterStack<T>(object stackChoice) where T : class, IStack
        {
            _injection.Register<T>();
            _postRun.Add(() => { _injection.Get<IStackRunner>().RegisterStack<T>(stackChoice); });
        }

        


    }
}
