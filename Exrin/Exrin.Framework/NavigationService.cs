namespace Exrin.Framework
{
    using Abstraction;
    using Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class NavigationService : INavigationService
    {
        private readonly IViewService _viewService = null;
        private bool _showNavigationBar = false;
        private readonly INavigationState _state = null;
        private INavigationContainer _navigationContainer = null;
        private static AsyncLock _lock = new AsyncLock();
        private readonly Dictionary<string, Type> _viewsByKey = new Dictionary<string, Type>();
        private object _stackIdentifier = null;
        private readonly IDictionary<object, IList<string>> _stackBasedViewKeyTracking = new Dictionary<object, IList<string>>();

        public NavigationService(IViewService viewService, INavigationState state)
        {
            _viewService = viewService;
            _state = state;
        }

        public async Task GoBack(object parameter)
        {
            await _navigationContainer.PopAsync(parameter);
        }

        public async Task GoBack()
        {
            await _navigationContainer.PopAsync();
        }

        public virtual void Init(object stackIdentifier, INavigationContainer container, bool showNavigationBar)
        {
            _stackIdentifier = stackIdentifier;

            if (!_stackBasedViewKeyTracking.Keys.Contains(stackIdentifier)) // Start ViewKey Tracking for the stack
                _stackBasedViewKeyTracking.Add(stackIdentifier, new List<string>());

            if (_navigationContainer != null)
                _navigationContainer.OnPopped -= container_OnPopped;

            container.OnPopped += container_OnPopped;

            _navigationContainer = container;
            _showNavigationBar = showNavigationBar;
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

            // Remove CurrentViewKey
            _stackBasedViewKeyTracking[_stackIdentifier].Remove(_navigationContainer.CurrentViewKey);

            // Changes the navigation key back to the previous page
            _navigationContainer.CurrentViewKey = _viewsByKey.First(x => x.Value == e.CurrentView.GetType()).Key;
        }

        private string BuildKey(string key)
        {
            return BuildKey(_stackIdentifier, key);
        }

        private string BuildKey(object stackIdentifier, string key)
        {
            if (stackIdentifier == null)
                throw new NullReferenceException($"{nameof(stackIdentifier)} is null when trying to build the mapping key");

            if (string.IsNullOrEmpty(key))
                return string.Empty;

            return $"{stackIdentifier}_{key}";
        }

        public virtual void Map(object stackIdentifier, string key, Type viewType, Type viewModelType)
        {
            lock (_viewsByKey)
            {
                key = BuildKey(stackIdentifier, key);

                // Map Key with View
                if (!string.IsNullOrEmpty(key))
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

                viewKey = BuildKey(viewKey);

                // Do not navigate to the same view.
                if (viewKey == _navigationContainer.CurrentViewKey)
                {
                    // TODO: Cleanup - push parameter again - this isn't the way to do it
                    var view = _navigationContainer.CurrentView as IView;
                    if (view != null)
                        ThreadHelper.RunOnUIThread(() =>
                        {
                            var model = view.BindingContext as IViewModel;
                            if (model != null)
                                model.OnNavigated(args); // Do not await.
                        });

                    return;
                }
                
                if (_viewsByKey.ContainsKey(viewKey))
                {
                    var type = _viewsByKey[viewKey];

                    var view = await _viewService.Build(type, args) as IView;

                    if (view == null)
                        throw new Exception(String.Format("Unable to build view {0}", type.ToString()));

                    if (_navigationContainer == null)
                        throw new Exception($"{nameof(INavigationContainer)} is null. Did you forget to call NavigationService.Init()?");

                    _navigationContainer.SetNavigationBar(_showNavigationBar, view);
                    
                    if (_stackBasedViewKeyTracking[_stackIdentifier].Contains(viewKey))
                    {
                        // Pop until we get back to that page
                        while (viewKey != _navigationContainer.CurrentViewKey)
                            await _navigationContainer.PopAsync();
                    }
                    else
                    {
                        var model = view.BindingContext as IViewModel;

                        if (model != null)
                        {
                            view.Appearing += (s, e) => { model.OnAppearing(); };
                            view.Disappearing += (s, e) => { model.OnDisappearing(); };
                            view.OnBackButtonPressed = () => { return model.OnBackButtonPressed(); };
                        }

                        await _navigationContainer.PushAsync(view);
                        _stackBasedViewKeyTracking[_stackIdentifier].Add(viewKey);

                        _navigationContainer.CurrentViewKey = viewKey;
                        _state.ViewName = viewKey;

                        ThreadHelper.RunOnUIThread(() =>
                        {
                            if (model != null)
                                model.OnNavigated(args); // Do not await.
                        });                        
                    }

                }
                else
                {
                    throw new ArgumentException(
                            $"No such key: {viewKey}. Did you forget to call NavigationService.Map?",
                            nameof(viewKey));
                }
            }
        }
        /// <summary>
        /// WARNING: I shouldn't be exposing this. Please don't base anything off this it will be refactored later
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<object> BuildView(string key, object args)
        {
            key = BuildKey(key);
            IView view = null;
            if (_viewsByKey.ContainsKey(key))
            {
                var type = _viewsByKey[key];

                view = await _viewService.Build(type, args) as IView;

                if (view == null)
                    throw new Exception($"Unable to build view {type.ToString()}");
            }

            return view;
        }

        public async Task LoadStack(Dictionary<string, object> definitions)
        {
            foreach (var page in definitions)
                await Navigate(page.Key, page.Value);
        }
    }
}
