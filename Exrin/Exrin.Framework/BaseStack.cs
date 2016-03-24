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


        public BaseStack(INavigationService navigationService)
        {
            _navigationService = navigationService;            
        }

        public void Init()
        {
            MapPages();
            MapViewModels();
        }

        public bool ShowNavigationBar { get; set; } = true;

        public INavigationPage Container { get; private set; }

        public StackStatus Status { get; set; } = StackStatus.Stopped;

        public async Task StartNavigation(object args = null)
        {
            await _navigationService.Navigate(NavigationStartPageKey, args);

            Status = StackStatus.Started;
        }

        /// <summary>
        /// Will register appropriate Services for Dependency Injection.
        /// </summary>
        protected void SetContainer(INavigationPage container)
        {
            Container = container;
        }

        protected virtual void MapPages() { }
        protected virtual void MapViewModels() { }
        protected virtual string NavigationStartPageKey { get; }

    }
}
