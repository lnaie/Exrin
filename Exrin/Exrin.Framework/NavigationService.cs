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

        public async Task GoBack(object parameter)
        {
            await _page.PopAsync(parameter);
        }

        public async Task GoBack()
        {
            await _page.PopAsync();
        }

        public virtual void Init(INavigationPage page)
        {
            if (_page != null)
                _page.OnPopped -= page_OnPopped;

            page.OnPopped += page_OnPopped;

            _page = page;
        }

        private void page_OnPopped(object sender, IPageNavigationArgs e)
        {
            if (e.PoppedPage != null)
            {
                var model = e.PoppedPage.BindingContext as IViewModel;
                if (model != null)
                    model.OnPopped();

                var disposableModel = e.PoppedPage.BindingContext as IDisposable;
                if (disposableModel != null)
                    disposableModel.Dispose();
            }

            if (e.CurrentPage != null)
            {
                var model = e.CurrentPage.BindingContext as IViewModel;
                if (model != null)
                    model.OnBackNavigated(null);
            }

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

                    var page = await _pageService.Build(type, args) as IPage;

                    if (page == null)
                        throw new Exception(String.Format("Unable to build page {0}", type.ToString()));

                    if (_page == null)
                        throw new Exception("INavigationPage is null. Did you forget to call NavigationService.Init()?");

                    _page.SetNavigationBar(false, page); //TODO: read from stack

                    var model = page.BindingContext as IViewModel;

                    if (model != null)
                    {
                        page.Appearing += (s, e) => { model.OnAppearing(); };
                        page.Disappearing += (s, e) => { model.OnDisappearing(); };
                    }

                    await _page.PushAsync(page);

                    ThreadHelper.RunOnUIThread(() =>
                    {
                        if (model != null)
                            model.OnNavigated(args); // Do not await.
                    });
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
