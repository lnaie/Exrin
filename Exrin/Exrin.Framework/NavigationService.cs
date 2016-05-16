using Exrin.Abstraction;
using Exrin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class NavigationService : INavigationService
    {
        private readonly IViewService _viewService = null;
        private readonly IApplicationInsights _applicationInsights = null;
        private INavigationContainer _navigationContainer = null;
        private static AsyncLock _lock = new AsyncLock();
        private readonly Dictionary<string, Type> _viewsByKey = new Dictionary<string, Type>();

        public NavigationService(IViewService viewService, IApplicationInsights applicationInsights)
        {
            _viewService = viewService;
            _applicationInsights = applicationInsights;
        }

        public async Task GoBack(object parameter)
        {
            await _navigationContainer.PopAsync(parameter);
        }

        public async Task GoBack()
        {
            await _navigationContainer.PopAsync();
        }

        public virtual void Init(INavigationContainer container)
        {
            if (_navigationContainer != null)
                _navigationContainer.OnPopped -= container_OnPopped;

            container.OnPopped += container_OnPopped;

            _navigationContainer = container;
        }

        private void container_OnPopped(object sender, IViewNavigationArgs e)
        {
            if (e.PoppedView != null)
            {
                var model = e.PoppedView.BindingContext as IViewModel;
                if (model != null)
                    model.OnPopped();

                var disposableModel = e.PoppedView.BindingContext as IDisposable;
                if (disposableModel != null)
                    disposableModel.Dispose();
            }

            if (e.CurrentView != null)
            {
                var model = e.CurrentView.BindingContext as IViewModel;
                if (model != null)
                    model.OnBackNavigated(null);
            }

        }

        public virtual void Map(string key, Type viewType, Type viewModelType)
        {
            lock (_viewsByKey)
            {
                // Map Key with View
                if (!String.IsNullOrEmpty(key))
                    if (_viewsByKey.ContainsKey(key))
                        _viewsByKey[key] = viewType;
                    else
                        _viewsByKey.Add(key, viewType);

                // Map View and ViewModel
                _viewService.Map(viewType, viewModelType);
            }
        }

        public async Task Navigate(string key)
        {
            await Navigate(key, null);
        }

        public async Task Navigate(string viewKey, object args)
        {
            using (var releaser = await _lock.LockAsync())
            {
                // Do not navigate to the same view.
                if (viewKey == _navigationContainer.CurrentViewKey)
                    return;

                _navigationContainer.CurrentViewKey = viewKey;

                if (_viewsByKey.ContainsKey(viewKey))
                {
                    var type = _viewsByKey[viewKey];

                    var view = await _viewService.Build(type, args) as IView;

                    if (view == null)
                        throw new Exception(String.Format("Unable to build view {0}", type.ToString()));

                    if (_navigationContainer == null)
                        throw new Exception($"{nameof(INavigationContainer)} is null. Did you forget to call NavigationService.Init()?");

                    _navigationContainer.SetNavigationBar(false, view); //TODO: read from stack

                    var model = view.BindingContext as IViewModel;

                    if (model != null)
                    {
                        view.Appearing += (s, e) => { model.OnAppearing(); };
                        view.Disappearing += (s, e) => { model.OnDisappearing(); };
                    }

                    await _navigationContainer.PushAsync(view);

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
                            "No such key: {0}. Did you forget to call NavigationService.Map?",
                            viewKey),
                        "key");
                }
            }
        }
    }
}
