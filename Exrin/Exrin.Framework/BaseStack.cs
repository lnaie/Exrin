namespace Exrin.Framework
{
    using Abstraction;
    using Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BaseStack : IStack
    {
        private static AsyncLock _lock = new AsyncLock();
        private readonly Dictionary<string, TypeDefinition> _viewsByKey = new Dictionary<string, TypeDefinition>();
        private readonly IList<string> _viewKeyTracking = new List<string>();
        private readonly IViewService _viewService;

        public object StackIdentifier { get; private set; }
        public StackStatus Status { get; private set; } = StackStatus.Stopped;
        public bool ShowNavigationBar { get; set; }
        public string CurrentViewKey { get; set; }
        private IList<IView> CurrentViewTrack = new List<IView>();
        public INavigationProxy Proxy { get; private set; }
        public virtual string NavigationStartKey { get; }

        public BaseStack(INavigationProxy navigationProxy, IViewService viewService, object stackIdentifier)
        {
            Proxy = navigationProxy;
            Proxy.OnPopped += proxy_OnPopped;
            StackIdentifier = stackIdentifier;
            _viewService = viewService;
        }

        public void Init()
        {
            Map();
        }

        protected virtual void Map() { }

        public async Task Navigate<TViewModel>(object args) where TViewModel : class, IViewModel
        {
            var viewKey = GetViewKey<TViewModel>();

            if (string.IsNullOrEmpty(viewKey))
                return;

            await Navigate(viewKey, args);
        }

        private string GetViewKey<TViewModel>() where TViewModel : class, IViewModel
        {
            var type = typeof(TViewModel);
            var viewType = _viewService.GetMap(type);
            foreach (var view in _viewsByKey)
                if (view.Value.Type == viewType)
                    return view.Key;

            throw new NullReferenceException($"There is no ViewModel type {typeof(TViewModel)}");
        }

        /// <summary>
        /// Will map the View, ViewModel to a key
        /// </summary>
        protected virtual void NavigationMap<View, ViewModel>(string key, IMapOptions options = null) where View : IView
                                                                                                  where ViewModel : IViewModel
        {
            lock (_viewsByKey)
            {
                var noHistory = options == null ? false : options.NoHistory;
                var cacheView = options == null ? false : options.CacheView;
                // Map Key with View
                if (!string.IsNullOrEmpty(key))
                {
                    var definition = new TypeDefinition() { Type = typeof(View), NoHistory = noHistory, CacheView = cacheView };
                    if (_viewsByKey.ContainsKey(key))
                        _viewsByKey[key] = definition;
                    else
                        _viewsByKey.Add(key, definition);
                }
                // Map View and ViewModel
                _viewService.Map(typeof(View), typeof(ViewModel));
            }
        }

        public async Task StartNavigation(object args = null, bool loadStartKey = true)
        {
            if (loadStartKey)
                await Navigate(NavigationStartKey, args);

            Status = StackStatus.Started;
        }

        public async Task GoBack(object parameter)
        {
            using (var releaser = await _lock.LockAsync())
            {
                await ThreadHelper.RunOnUIThreadAsync(async () =>
                {
                    await Proxy.PopAsync(parameter);
                });
            }
        }

        public async Task GoBack()
        {
            using (var releaser = await _lock.LockAsync())
            {
                await ThreadHelper.RunOnUIThreadAsync(async () =>
                {
                    await Proxy.PopAsync();
                });
            }
        }

        public async Task Navigate(string key, object args)
        {
            using (var releaser = await _lock.LockAsync())
            {
                await ThreadHelper.RunOnUIThreadAsync(async () =>
                {
                    // Do not navigate to the same view.
                    if (key == CurrentViewKey)
                    {
                        var model = CurrentViewTrack[CurrentViewTrack.Count - 1].BindingContext as IViewModel;

                        if (model != null)
                            model.OnNavigated(args).ConfigureAwait(false).GetAwaiter(); // Do not await.
                        
                        return;
                    }

                    if (_viewsByKey.ContainsKey(key))
                    {
                        var typeDefinition = _viewsByKey[key];

                        var view = await _viewService.Build(typeDefinition) as IView;

                        if (view == null)
                            throw new Exception(String.Format("Unable to build view {0}", typeDefinition.Type.ToString()));

                        if (Proxy == null)
                            throw new Exception($"{nameof(INavigationProxy)} is null. Did you forget to call NavigationService.Init()?");

                        Proxy.SetNavigationBar(ShowNavigationBar, view);

                        if (_viewKeyTracking.Contains(key))
                        {
                            // Pop until we get back to that page
                            while (key != CurrentViewKey)
                                await Proxy.PopAsync();
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

                            var popCurrent = false;

                            if (Proxy != null && !string.IsNullOrEmpty(CurrentViewKey))
                                if (_viewsByKey[CurrentViewKey].NoHistory)
                                    popCurrent = true;


                            if (model != null)
                                await model.OnPreNavigate(args);

                            await Proxy.PushAsync(view);

                            if (popCurrent) // Pop the one behind without showing it
                            {
                                await Proxy.SilentPopAsync(-1);
                                // Remove the top one as the new tracking key hasn't been added yet
                                _viewKeyTracking.RemoveAt(_viewKeyTracking.Count - 1);
                            }

                            _viewKeyTracking.Add(key);

                            CurrentViewKey = key;

                            CurrentViewTrack.Add(view);

                            if (model != null)
                                model.OnNavigated(args).ConfigureAwait(false).GetAwaiter(); // Do not await.

                        }
                    }
                    else
                    {
                        throw new ArgumentException(
                                $"No such key: {key}. Did you forget to call NavigationService.Map?",
                                nameof(key));
                    }
                });
            }
        }

        private void proxy_OnPopped(object sender, IViewNavigationArgs e)
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
            _viewKeyTracking.Remove(CurrentViewKey);

            // Changes the navigation key back to the previous page
            CurrentViewKey = _viewsByKey.First(x => x.Value.Type == e.CurrentView.GetType()).Key;
            CurrentViewTrack.RemoveAt(CurrentViewTrack.Count - 1);
        }

        private void NoHistoryRemoval()
        {
            if (Proxy != null && !string.IsNullOrEmpty(CurrentViewKey))
                if (_viewsByKey[CurrentViewKey].NoHistory)
                    StackChangeActions.Add(new Action(() =>
                    {
                        ThreadHelper.RunOnUIThread(async () =>
                        {
                            using (var releaser = await _lock.LockAsync())
                            {
                                await Proxy.SilentPopAsync(0);

                            }
                        });
                    }));
        }

        private IList<Action> StackChangeActions = new List<Action>();

        public Task StackChanged()
        {
            foreach (var action in StackChangeActions)
                action();

            StackChangeActions.Clear();

            return Task.FromResult(true);
        }

        public void StateChange(StackStatus state)
        {
            switch (state)
            {
                case StackStatus.Stopped:
                case StackStatus.Background:
                    Proxy.ViewStatus = VisualStatus.Hidden;
                    Status = StackStatus.Background;
                    // Removes No History from last page of previous stack
                    NoHistoryRemoval();
                    break;
                case StackStatus.Started:
                    Status = StackStatus.Started;
                    break;
            }

        }

    }

}