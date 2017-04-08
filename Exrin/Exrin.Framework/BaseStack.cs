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
        private readonly Dictionary<Abstraction.Tuple<string, string>, TypeDefinition> _viewsByKey = new Dictionary<Abstraction.Tuple<string, string>, TypeDefinition>();
        private readonly IList<Abstraction.Tuple<string, string>> _viewKeyTracking = new List<Abstraction.Tuple<string, string>>();
        private readonly IViewService _viewService;

        public object StackIdentifier { get; private set; }
        public StackStatus Status { get; private set; } = StackStatus.Stopped;
        public bool ShowNavigationBar { get; set; }
        private Abstraction.Tuple<string, string> CurrentView { get; set; }
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

            await Navigate(viewKey.Key, args);
        }

        private Abstraction.Tuple<string, string> GetViewKey<TViewModel>() where TViewModel : class, IViewModel
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
                var platform = options == null ? null : options.Platform;

                // Map Key with View
                if (!string.IsNullOrEmpty(key))
                {
                    var definition = new TypeDefinition() { Type = typeof(View), NoHistory = noHistory, CacheView = cacheView, Platform = platform };
                    var tupleKey = Abstraction.Tuple.Create(key, platform);
                    if (_viewsByKey.ContainsKey(tupleKey))
                        _viewsByKey[tupleKey] = definition;
                    else
                        _viewsByKey.Add(tupleKey, definition);
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
        public Task Navigate(string key, object args)
        {
            return Navigate(key, args, false, false);
        }

        public async Task Navigate(string key, object args, bool newInstance, bool popSource)
        {
            using (var releaser = await _lock.LockAsync())
            {
                await ThreadHelper.RunOnUIThreadAsync(async () =>
                {
                    // Do not navigate to the same view, unless duplicate
                    if (key == CurrentView.Key && !newInstance)
                    {
                        var model = CurrentViewTrack[CurrentViewTrack.Count - 1].BindingContext as IViewModel;

                        if (model != null)
                            model.OnNavigated(args).ConfigureAwait(false).GetAwaiter(); // Do not await.

                        return;
                    }

                    var platformKey = Abstraction.Tuple.Create(key, App.PlatformOptions.Platform);
                    var genericKey = Abstraction.Tuple.Create(key, (string)null);

                    Abstraction.Tuple<string, string> tupleKey = Abstraction.Tuple.Create(string.Empty, (string)null);

                    TypeDefinition viewKey = null;

                    if (_viewsByKey.ContainsKey(platformKey))
                    {
                        tupleKey = platformKey;
                        viewKey = _viewsByKey[platformKey];
                    }
                    else if (_viewsByKey.ContainsKey(genericKey))
                    {
                        tupleKey = genericKey;
                        viewKey = _viewsByKey[genericKey];
                    }

                    if (viewKey != null)
                    {
                        var typeDefinition = viewKey;
                        
                        if (Proxy == null)
                            throw new Exception($"{nameof(INavigationProxy)} is null. Did you forget to call NavigationService.Init()?");
                                                

                        if (_viewKeyTracking.Contains(tupleKey) && !newInstance)
                        {                           
                            // Silent pop those in the middle, then do a pop, so its a single back animation according to the user
                            var index = 0;
                            foreach (var item in _viewKeyTracking)
                                if (item.Key != key)
                                    index += 1;
                                else
                                    break;
                            var count = _viewKeyTracking.Count;
                            for (int i = count - 2; i > index; i--)
                            {
                                await Proxy.SilentPopAsync(i);
                                _viewKeyTracking.RemoveAt(i);
                            }

                            // Now should be single pop to go back to the page.
                            while (key != CurrentView.Key)
                                await Proxy.PopAsync();
                        }
                        else
                        {
                            var view = await _viewService.Build(typeDefinition) as IView;

                            if (view == null)
                                throw new Exception(String.Format("Unable to build view {0}", typeDefinition.Type.ToString()));

                            Proxy.SetNavigationBar(ShowNavigationBar, view);

                            var model = view.BindingContext as IViewModel;

                            if (model != null)
                            {
                                var arg = new Args();
                                await model.OnPreNavigate(args, arg);

                                // If user cancelled, stop forward navigation
                                if (arg.Cancel)
                                    return;

                                view.Appearing += (s, e) => { model.OnAppearing(); };
                                view.Disappearing += (s, e) => { model.OnDisappearing(); };
                                view.OnBackButtonPressed = () => { return model.OnBackButtonPressed(); };
                            }

                            var popCurrent = false;

                            if (Proxy != null && !string.IsNullOrEmpty(CurrentView.Key))
                                if (_viewsByKey[CurrentView].NoHistory)
                                    popCurrent = true;

                            await Proxy.PushAsync(view);

                            if (popCurrent || popSource) // Pop the one behind without showing it
                            {
                                await Proxy.SilentPopAsync(1);
                                // Remove the top one as the new tracking key hasn't been added yet
                                _viewKeyTracking.RemoveAt(_viewKeyTracking.Count - 1);
                            }

                            _viewKeyTracking.Add(tupleKey);

                            CurrentView = tupleKey;

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
                    model.OnBackNavigated(e.Parameter);
            }

            // Remove CurrentView
            _viewKeyTracking.Remove(CurrentView);

            // Changes the navigation key back to the previous page
            CurrentView = _viewsByKey.First(x => x.Value.Type == e.CurrentView.GetType()).Key;
            CurrentViewTrack.RemoveAt(CurrentViewTrack.Count - 1);
        }

        private void NoHistoryRemoval()
        {
            if (Proxy != null && !string.IsNullOrEmpty(CurrentView.Key))
                if (_viewsByKey[CurrentView].NoHistory)
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

        public async Task SilentPop(IList<string> viewKeys)
        {
            foreach (var key in viewKeys)
            {
                var tuple = Abstraction.Tuple.Create(key, App.PlatformOptions?.Platform);

                // Get index in stack
                var index = _viewKeyTracking.IndexOf(_viewKeyTracking.FirstOrDefault(x => x.Key == key));
                var indexFromTop = _viewKeyTracking.Count - index - 1;
                await Proxy.SilentPopAsync(indexFromTop);

                // Remove from key tracking
                _viewKeyTracking.RemoveAt(index);
            }
        }
    }

}