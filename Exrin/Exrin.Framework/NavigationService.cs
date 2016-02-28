using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class NavigationService : INavigationService
    {
        private readonly IPageService _pageService = null;
        private INavigationPage _page = null;

        public NavigationService(IPageService pageService)
        {
            _pageService = pageService;
        }

        public async Task GoBack()
        {
            await _page.PopAsync();
        }

        public void Init(INavigationPage page)
        {
            _page = page;
        }

        public async Task Navigate(string pageKey)
        {
            // Get mapping

           await _page.PushAsync(await _pageService.Build(pageKey, null));
        }

        public Task Navigate(string pageKey, object args)
        {
            throw new NotImplementedException();
        }
    }
}
