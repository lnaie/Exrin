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
        //protected INavigationPage _container = null;
        protected INavigationService _navigationService = null;
        private IDisplayService _displayService = null;
        protected IPageService _pageService = null;

        public BaseStack(INavigationService navigationService, IDisplayService dialogService, IPageService pageService)
        {
            _navigationService = navigationService;
            _displayService = dialogService;
            _pageService = pageService;

            MapPages();
            MapViewModels();
        }

        public async Task StartNavigation(object args = null)
        {
            await _navigationService.Navigate(NavigationStartPageKey, args);
        }

        /// <summary>
        /// Will register appropriate Services for Dependency Injection.
        /// </summary>
        protected void SetContainer(INavigationPage container)
        {
            _navigationService.Init(container);
            _displayService.Init(container);
        }

        protected virtual void MapPages() { }
        protected virtual void MapViewModels() { }
        protected virtual string NavigationStartPageKey { get; }

    }
}
