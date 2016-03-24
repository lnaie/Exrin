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
        private static AsyncLock _lock = new AsyncLock();
        private string _currentPageKey = "";
        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();

        public NavigationService(IPageService pageService)
        {
            _pageService = pageService;
        }

        public async Task GoBack()
        {
            await _page.PopAsync();
        }

        public virtual void Init(INavigationPage page)
        {
            _page = page;
        }

        public virtual void Map(string pageKey, Type pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(pageKey))
                    _pagesByKey[pageKey] = pageType;
                else
                    _pagesByKey.Add(pageKey, pageType);
            }
        }

        public async Task Navigate(string pageKey)
        {
            await Navigate(pageKey, null);
        }

        public async Task Navigate(string pageKey, object args)
        {
            using (var releaser = await _lock.LockAsync())
            {
                // Do not navigate to the same page.
                if (pageKey == _currentPageKey)
                    return;

                if (_pagesByKey.ContainsKey(pageKey))
                {
                    var type = _pagesByKey[pageKey];

                    var page = await _pageService.Build(type, args);

                    if (page == null)
                        throw new Exception(String.Format("Unable to build page {0}", type.ToString()));

                    if (_page == null)
                        throw new Exception("Navigation Page is null. Did you forget to call NavigationService.Init?");

                    _page.SetNavigationBar(false, page); //TODO: read from stack
                                        
                    await _page.PushAsync(page);
                }
                else
                {
                    throw new ArgumentException(
                        string.Format(
                            "No such page: {0}. Did you forget to call NavigationService.Map?",
                            pageKey),
                        "pageKey");
                }
            }
        }
    }
}
