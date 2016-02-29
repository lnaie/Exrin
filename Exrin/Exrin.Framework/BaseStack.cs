using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
        
    public class BaseStack
    {
        protected INavigationPage _container = null;
        protected INavigationService _navigationService = null;
        private IDisplayService _displayService = null;
        protected IPageService _pageService = null;

        private bool _isFirstRun = false;

        public BaseStack(INavigationService navigationService, IDisplayService dialogService, IPageService pageService)
        {
            _navigationService = navigationService;
            _displayService = dialogService;
            _pageService = pageService;
        }

        /// <summary>
        /// Will register appropriate Services for Dependency Injection.
        /// </summary>
        public async Task Init(object navigationArgs = null)
        {

            _navigationService.Init(_container);
            _displayService.Init(_container);

            if (!_isFirstRun)
            {
                MapPages();
                MapViewModels();

                await _navigationService.Navigate(NavigationStartPageKey, navigationArgs);

                _isFirstRun = true;
            }

        }

        protected virtual void MapPages() { }
        protected virtual void MapViewModels() { }
        protected virtual string NavigationStartPageKey { get; }

    }
}
