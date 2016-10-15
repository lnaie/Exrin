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
        private readonly Dictionary<string, TypeDefinition> _viewsByKey = new Dictionary<string, TypeDefinition>();
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
            // Removes No History from last page of previous stack
            NoHistoryRemoval(_navigationContainer, _stackIdentifier);

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

            var container = sender as INavigationContainer;

            // Remove CurrentViewKey
            _stackBasedViewKeyTracking[_stackIdentifier].Remove(container.CurrentViewKey);

            // Changes the navigation key back to the previous page
            container.CurrentViewKey = _viewsByKey.First(x => x.Value.Type == e.CurrentView.GetType()).Key;
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

        public virtual void Map(object stackIdentifier, string key, Type viewType, Type viewModelType, bool noHistory = false)
        {
            lock (_viewsByKey)
            {
                key = BuildKey(stackIdentifier, key);

                // Map Key with View
                if (!string.IsNullOrEmpty(key))
                    if (_viewsByKey.ContainsKey(key))
                        _viewsByKey[key] = new TypeDefinition() { Type = viewType, NoHistory = noHistory };
                    else
                        _viewsByKey.Add(key, new TypeDefinition() { Type = viewType, NoHistory = noHistory });

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
                    var typeDefinition = _viewsByKey[viewKey];

                    var view = await _viewService.Build(typeDefinition.Type) as IView;

                    if (view == null)
                        throw new Exception(String.Format("Unable to build view {0}", typeDefinition.Type.ToString()));

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

                        var popCurrent = false;

                        if (_navigationContainer != null && !string.IsNullOrEmpty(_navigationContainer.CurrentViewKey))
                            if (_viewsByKey[_navigationContainer.CurrentViewKey].NoHistory)
                                popCurrent = true;

                        await _navigationContainer.PushAsync(view);

                        if (popCurrent) // Pop the one behind without showing it
                            ThreadHelper.RunOnUIThread(async () =>
                            {
                                await _navigationContainer.SilentPopAsync(-1);
                                // Remove the top one as the new tracking key hasn't been added yet
                                _stackBasedViewKeyTracking[_stackIdentifier].RemoveAt(_stackBasedViewKeyTracking[_stackIdentifier].Count - 1);
                            });

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

        private IList<Action> StackChangeActions = new List<Action>();

        /// <summary>
        /// If No History, we need to Pop before moving forward
        /// </summary>
        private void NoHistoryRemoval(INavigationContainer container, object stackIdentifier)
        {

            if (stackIdentifier == null)
                return;

            if (container != null && !string.IsNullOrEmpty(container.CurrentViewKey))
                if (_viewsByKey[container.CurrentViewKey].NoHistory)
                    StackChangeActions.Add(new Action(() =>
                    {
                        ThreadHelper.RunOnUIThread(async () =>
                        {                           
                            await container.SilentPopAsync(0);
                            var count = _stackBasedViewKeyTracking[stackIdentifier].Count;
                            _stackBasedViewKeyTracking[stackIdentifier].RemoveAt(count - 1);
                            container.CurrentViewKey = _stackBasedViewKeyTracking[stackIdentifier][count -2];
                        });
                    }));
        }

        public async Task<object> BuildView(string key, object args)
        {
            key = BuildKey(key);
            IView view = null;
            if (_viewsByKey.ContainsKey(key))
            {
                var type = _viewsByKey[key];

                view = await _viewService.Build(type.Type) as IView;

                if (view == null)
                    throw new Exception($"Unable to build view {type.ToString()}");
            }

            return view;
        }
        
        public Task StackChanged()
        {
            foreach (var action in StackChangeActions)
                action();

            StackChangeActions.Clear();

            return Task.FromResult(true);
        }
    }
}
