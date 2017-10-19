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
		private readonly INavigationState _state = null;
		private readonly IDictionary<object, IStack> _stacks = new Dictionary<object, IStack>();
		private readonly IDictionary<object, object> _stackViewContainers = new Dictionary<object, object>();
		private readonly IDictionary<object, IViewContainer> _viewContainers = new Dictionary<object, IViewContainer>();
		private object _currentStack = null;
		private IViewContainer _currentViewContainer = null;
		private readonly IDisplayService _displayService;
		private readonly IInjectionProxy _injection;
		private Action<object> _setRoot = null;
		private readonly object _lock = new object();
		private Func<object> _getRoot;

		public NavigationService(IViewService viewService, INavigationState state, IInjectionProxy injection, IDisplayService displayService)
		{
			_viewService = viewService;
			_state = state;
			_displayService = displayService;
			_injection = injection;
		}

		public async Task Navigate(string key)
		{
			await Navigate(viewKey: key, args: null);
		}
		public Task Navigate(string viewKey, object args)
		{
			return Navigate(viewKey, args, false);
		}
		public Task Navigate(string viewKey, object args, bool newInstance)
		{
			return Navigate(viewKey, args, newInstance, false);
		}
		public async Task Navigate(string viewKey, object args, bool newInstance, bool popSource)
		{
			// Navigate on Current Stack
			await _stacks[_currentStack].Navigate(viewKey, args, newInstance, popSource);

			_state.ViewName = viewKey;
		}
		public async Task Navigate<TViewModel>(object args) where TViewModel : class, IViewModel
		{
			await _stacks[_currentStack].Navigate<TViewModel>(args);
		}

		public async Task Navigate(object stackIdentifier, string viewKey, object args)
		{
			Navigate(new StackOptions() { StackChoice = stackIdentifier });

			await Navigate(viewKey, args);
		}

		public async Task Navigate(string viewKey, object args, IStackOptions options)
		{
			Navigate(options);

			await Navigate(viewKey, args);
		}

		public async Task Navigate(object containerId, object regionId, object stackIdentifier, string viewKey, object args)
		{
			Navigate(containerId, regionId, new StackOptions() { StackChoice = stackIdentifier });

			await Navigate(viewKey, args);
		}

		public async Task Navigate(object containerId, object regionId, string viewKey, object args, IStackOptions options)
		{
			Navigate(containerId, regionId, options);

			await Navigate(viewKey, args);
		}

		[Obsolete()]
		public void Init(Action<object> setRoot)
		{
			_setRoot = setRoot;
		}

		public void Init(Action<object> setRoot, Func<object> getRoot)
		{
			_getRoot = getRoot;
			_setRoot = setRoot;
		}

		public async Task GoBack()
		{
			await _stacks[_currentStack].GoBack();
		}

		public async Task GoBack(object parameter)
		{
			await _stacks[_currentStack].GoBack(parameter);
		}

		public void RegisterViewContainer<T>() where T : class, IViewContainer
		{
			_injection.Register<T>(InstanceType.SingleInstance);

			var viewContainer = _injection.Get<T>();
			IList<IStack> stacks = new List<IStack>();

			// Load list of stacks depending on ViewContainer
			if (viewContainer as ISingleContainer != null)
			{
				stacks.Add(((ISingleContainer)viewContainer).Stack);
			}
			else if (viewContainer as IMasterDetailContainer != null)
			{
				var mdpContainer = (IMasterDetailContainer)viewContainer;

				if (mdpContainer.MasterStack is IStack masterStack)
					stacks.Add(masterStack);
				else if (mdpContainer.MasterStack is ITabbedContainer masterViewContainer)
					foreach (var stack in masterViewContainer.Children)
						stacks.Add(stack);

				if (mdpContainer.DetailStack is IStack detailStack)
					stacks.Add(detailStack);
				else if (mdpContainer.DetailStack is ITabbedContainer detailViewContainer)
					foreach (var stack in detailViewContainer.Children)
						stacks.Add(stack);

			}
			else if (viewContainer as ITabbedContainer != null)
			{
				foreach (var stack in ((ITabbedContainer)viewContainer).Children)
					stacks.Add(stack);
			}
			else
			{
				throw new ArgumentException($"{nameof(T)} is not a valid {nameof(IViewContainer)}. Please use one of Exrin's default types");
			}

			// Register stacks and ViewContainer Associations
			foreach (var stack in stacks)
			{
				if (!_stacks.ContainsKey(stack.StackIdentifier))
					_stacks.Add(stack.StackIdentifier, stack);

				if (!_stackViewContainers.ContainsKey(stack.StackIdentifier))
					_stackViewContainers.Add(stack.StackIdentifier, viewContainer.Identifier);
			}

			// Register ViewContainers
			if (!_viewContainers.ContainsKey(viewContainer.Identifier))
				_viewContainers.Add(viewContainer.Identifier, viewContainer);
		}
		public void RegisterStack(IStack stack)
		{
			_stacks.Add(stack.StackIdentifier, stack);
		}

		public StackResult Navigate(IStackOptions options)
		{
			return Navigate(containerId: null, regionId: null, options: options);
		}
		public StackResult Navigate(object containerId, object regionId, IStackOptions options)
		{
			lock (_lock)
			{
				StackResult stackResult = StackResult.StackStarted;

				if (options.StackChoice == null)
					throw new NullReferenceException($"{nameof(NavigationService)}.{nameof(Navigate)} can not accept a null {nameof(options.StackChoice)}");

				// Don't change to the same stack
				if (_currentStack != null
					&& _currentStack.Equals(options.StackChoice))
				{
					if (_getRoot != null)
					{
						if (_getRoot() == null)
						{
							// Set Root Page
							ThreadHelper.RunOnUIThread(() =>
							{
								var viewContainer = _viewContainers[_stackViewContainers[options.StackChoice]];
								_setRoot?.Invoke(viewContainer?.NativeView);
							});
						}
					}

					return StackResult.None;
				}

				if (!_stacks.ContainsKey(options.StackChoice) && regionId == null)
					throw new NullReferenceException($"{nameof(NavigationService)} does not contain a stack named {options.StackChoice.ToString()}");

				// Current / Previous Stack
				IStack oldStack = null;
				if (_currentStack != null)
				{
					oldStack = _stacks[_currentStack];
					oldStack.StateChange(StackStatus.Background); // Schedules NoHistoryRemoval
				}

				var stack = _stacks[options.StackChoice];

				_currentStack = options.StackChoice;

				// Set new status
				stack.Proxy.ViewStatus = VisualStatus.Visible;

				// Switch over services
				_displayService.Init(stack.Proxy);

				ThreadHelper.RunOnUIThread(async () =>
				{
					if (stack.Status == StackStatus.Stopped)
					{
						object args = null;

						// If ArgsKey present only pass args along if the StartKey is the same
						if ((!string.IsNullOrEmpty(options?.ArgsKey) && stack.NavigationStartKey == options?.ArgsKey) || string.IsNullOrEmpty(options?.ArgsKey))
						{
							stackResult = stackResult | StackResult.ArgsPassed;
							args = options?.Args;
						}

						var loadStartKey = options?.PredefinedStack == null;

						if (loadStartKey)
							stackResult = stackResult | StackResult.NavigationStarted;

						await stack.StartNavigation(args: args, loadStartKey: loadStartKey);
					}

					//  Preload Stack
					if (options?.PredefinedStack != null)
						foreach (var page in options.PredefinedStack)
							await Navigate(page.Key, page.Value);

					IViewContainer viewContainer = null;

					// Find mainview from ViewHierarchy
					object viewContainerKey = regionId;
					if (containerId != null && regionId != null)
					{
						var container = _viewContainers[containerId.ToString()];
						if (container.RegionMapping.Any(x => x.Key.ToString() == regionId.ToString()))
							viewContainer = container;
					}
					else if (containerId != null && regionId == null)
					{
						var container = _viewContainers[containerId.ToString()];
						viewContainer = container;
					}
					else
					{
						if (_stackViewContainers.ContainsKey(stack.StackIdentifier))
							viewContainer = _viewContainers[_stackViewContainers[stack.StackIdentifier]];
						else
						{
							// Create single container
							viewContainer = new SingleViewContainer(Guid.NewGuid().ToString(), stack);

							// Add to list
							_viewContainers.Add(viewContainer.Identifier, viewContainer);
						}
					}

				
					// Tabbed View
					if (viewContainer is ITabbedContainer tabbedView)
						await InitializeTabbedView(tabbedView);

					var containerSwitch = viewContainer.RegionMapping.Any(x => x.Key.ToString() == Convert.ToString(regionId));
					KeyValuePair<object, ContainerType>? containerType = viewContainer.RegionMapping.FirstOrDefault(x => x.Key.ToString() == Convert.ToString(regionId));

					// MasterDetail View load
					if (viewContainer is IMasterDetailContainer)
					{
						var masterDetailContainer = viewContainer as IMasterDetailContainer;

						if (masterDetailContainer.DetailStack != null)
						{
							if (masterDetailContainer.DetailStack is IStack detailStackDefinition)
							{
								// Setup Detail Stack
								var detailStack = _stacks[detailStackDefinition.StackIdentifier];

								if (detailStack.Status == StackStatus.Stopped)
									await detailStack.StartNavigation(options?.Args);

								masterDetailContainer.Proxy.DetailNativeView = detailStack.Proxy.NativeView;
							}
							else if (masterDetailContainer.DetailStack is ITabbedContainer detailTabbedDefinition)
							{
								await InitializeTabbedView(detailTabbedDefinition);

								masterDetailContainer.Proxy.DetailNativeView = detailTabbedDefinition.NativeView;
							}

							if (masterDetailContainer.MasterStack is IStack masterStackDefinition)
							{
								// Setup Master Stack
								var masterStack = _stacks[masterStackDefinition.StackIdentifier];

								if (masterStack.Status == StackStatus.Stopped)
									await masterStack.StartNavigation(options?.Args);

								masterDetailContainer.Proxy.MasterNativeView = masterStack.Proxy.NativeView;
							}
							else if (masterDetailContainer.MasterStack is ITabbedContainer masterTabbedDefinition)
							{
								await InitializeTabbedView(masterTabbedDefinition);

								masterDetailContainer.Proxy.MasterNativeView = masterTabbedDefinition.NativeView;
							}
						}
					}

					if (viewContainer is ITabbedContainer tabbedContainer)
						// switch current page
						tabbedContainer.SetCurrentPage(stack);

					// If parent, then move switch container
					if (viewContainer.ParentContainer != null)
						viewContainer = viewContainer.ParentContainer;

					_currentViewContainer = viewContainer;

					if (!string.IsNullOrEmpty(options.ViewKey))
						await Navigate(options.ViewKey, options.Args, options.NewInstance);

					if (!containerSwitch)
						_setRoot?.Invoke(viewContainer?.NativeView);
					else if (viewContainer is IMasterDetailContainer container)
						container.SetStack(containerType.Value.Value, stack.Proxy.NativeView);

					if (oldStack != null)
						await oldStack.StackChanged();

				});

				return stackResult;
			}


			async Task InitializeTabbedView(ITabbedContainer tabbedView)
			{
				// Must start all stacks on the first tabbed view load
				// because when the tab changes, I can't block while I load an individual tab
				// I can only block moving to an entire page
				foreach (var item in tabbedView.Children)
					if (item.Status == StackStatus.Stopped)
						await item.StartNavigation(options?.Args);
			}
		}

		public async Task SilentPop(object stackIdentifier, IList<string> viewKeys)
		{
			var stack = _stacks[stackIdentifier];
			await stack.SilentPop(viewKeys);
		}
	}
}