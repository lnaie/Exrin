using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
        
    public class BaseStack: IStack
    {
        protected readonly INavigationService _navigationService = null;
        public object StackIdentifier { get; set; }

        public BaseStack(INavigationService navigationService, INavigationContainer navigationContainer, object stackIdentifier)
        {
            _navigationService = navigationService;
            SetContainer(navigationContainer);
            StackIdentifier = stackIdentifier;
        }

        public void Init()
        {
            Map();
        }

        public bool ShowNavigationBar { get; set; } = true;

        public INavigationContainer Container { get; private set; }

        public StackStatus Status { get; set; } = StackStatus.Stopped;

        public async Task StartNavigation(object args = null)
        {
            await _navigationService.Navigate(NavigationStartKey, args);

            Status = StackStatus.Started;
        }

        /// <summary>
        /// Will register appropriate Services for Dependency Injection.
        /// </summary>
        protected void SetContainer(INavigationContainer container)
        {
            Container = container;
        }

        protected virtual void Map() { }

		protected void NavigationMap(string key, Type view, Type viewModel) {
			_navigationService.Map(StackIdentifier, key, view, viewModel);
		}

		protected virtual string NavigationStartKey { get; }

       
    }
}
