namespace Exrin.Framework
{
	using Abstraction;
	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using System.Threading.Tasks;


	public abstract class ViewModel : BindableModel, IViewModel, IComposition
	{
		protected IExecution Execution { get; set; }

		[Obsolete("Please use NavigationService. Obsolete as of version 2.0.0.")]
		protected INavigationService _navigationService => NavigationService;
		[Obsolete("Please use ErrorHandlingService. Obsolete as of version 2.0.0.")]
		protected IErrorHandlingService _errorHandlingService => ErrorHandlingService;
		[Obsolete("Please use ApplicationInsights. Obsolete as of version 2.0.0.")]
		protected IApplicationInsights _applicationInsights => ApplicationInsights;
		[Obsolete("Please use DisplayService. Obsolete as of version 2.0.0.")]
		protected IDisplayService _displayService => DisplayService;

		protected INavigationService NavigationService { get; private set; }
		protected IErrorHandlingService ErrorHandlingService { get; private set; }
		protected IApplicationInsights ApplicationInsights { get; private set; }
		protected IDisplayService DisplayService { get; private set; }

		public ViewModel()
		{
			SetExecution();
		}
		void IComposition.SetContainer(IExrinContainer exrinContainer)
		{
			if (_containerSet)
				return;

			if (exrinContainer == null)
				throw new ArgumentNullException(nameof(IExrinContainer));

			ApplicationInsights = exrinContainer.ApplicationInsights;
			DisplayService = exrinContainer.DisplayService;
			NavigationService = exrinContainer.NavigationService;
			ErrorHandlingService = exrinContainer.ErrorHandlingService;

			_containerSet = true;
		}
		private bool _containerSet = false;

		public ViewModel(IVisualState visualState, [CallerFilePath] string caller = "ViewModel")
		{
			VisualState = visualState;

			SetExecution();
		}

		public ViewModel(IExrinContainer exrinContainer, IVisualState visualState, [CallerFilePath] string caller = "ViewModel")
		{

			if (exrinContainer == null)
				throw new ArgumentNullException(nameof(IExrinContainer));

			ApplicationInsights = exrinContainer.ApplicationInsights;
			DisplayService = exrinContainer.DisplayService;
			NavigationService = exrinContainer.NavigationService;
			ErrorHandlingService = exrinContainer.ErrorHandlingService;
			_containerSet = true;

			VisualState = visualState;

			SetExecution();

		}

		private void SetExecution()
		{
			Execution = new Execution()
			{
				HandleTimeout = TimeoutHandle,
				NotifyOfActivity = NotifyActivity,
				NotifyActivityFinished = NotifyActivityFinished,
				HandleResult = HandleResult,
				HandleUnhandledException = (e) => { return Task.FromResult(false); },
				PreCheck = null
			};
		}

		public VisualStatus ViewStatus { get; private set; } = VisualStatus.Unseen;

		private IVisualState _visualState;
		public IVisualState VisualState
		{
			get
			{
				return _visualState;
			}
			set
			{
				_visualState = value;

				Task.Run(() => _visualState?.Init())
					.ContinueWith((task) =>
					{
						if (task.Exception != null)
							ApplicationInsights?.TrackException(task.Exception);
					});
			}
		}


		private IDictionary<string, IRelayCommand> commands = new Dictionary<string, IRelayCommand>();
		public IRelayCommand GetCommand(Func<IRelayCommand> create, [CallerMemberName] string name = "")
		{
			// Stops getting the command, if Execution is null.
			if (Execution == null)
				return null;

			if (!commands.ContainsKey(name))
				commands.Add(name, create());

			return commands[name];
		}

		public virtual Task OnPreNavigate(object args, Args e)
		{
			return Task.FromResult(0);
		}

		public virtual Task OnNavigated(object args)
		{
			return Task.FromResult(0);
		}

		public virtual Task OnBackNavigated(object args)
		{
			return Task.FromResult(0);
		}

		public virtual void OnAppearing() { ViewStatus = VisualStatus.Visible; }

		public virtual void OnDisappearing() { ViewStatus = VisualStatus.Hidden; }

		public virtual void OnPopped()
		{
			ViewStatus = VisualStatus.Disposed;
		}

		public virtual bool OnBackButtonPressed() { return false; }

		public override void Disposing()
		{
			base.Disposing();

			var state = VisualState as BindableModel;
			state?.Dispose();
		}

		protected Func<Task> TimeoutHandle
		{
			get
			{
				return async () =>
				{
					await DisplayService.ShowDialog("Timeout", "Operation failed to complete within an acceptable amount of time");
				};
			}
		}

		/// <summary>
		/// Delay before setting IsBusy in milliseconds.
		/// Default is 400ms
		/// </summary>
		public int IsBusyDelay { get; set; } = 400;

		private object _lock = new object();
		private bool _isBusy = false;
		private bool _settingBusy = false;
		protected Func<Task> NotifyActivity
		{
			get
			{
				return () =>
				{
					lock (_lock)
						_isBusy = true;

					Task.Run(async () =>
					{
						lock (_lock)
							_settingBusy = true;

						await Task.Delay(IsBusyDelay);

						lock (_lock)
							if (_settingBusy)
								VisualState.IsBusy = _isBusy;
					});

					return Task.FromResult(0);

				};
			}
		}

		protected Func<Task> NotifyActivityFinished
		{
			get
			{
				return () =>
				{
					lock (_lock)
					{
						_isBusy = false;
						_settingBusy = false;
					}

					VisualState.IsBusy = _isBusy;

					return Task.FromResult(0);
				};
			}
		}

		protected virtual Func<IList<IResult>, Task> HandleResult
		{
			get
			{
				return async (results) =>
				{

					if (results == null)
						return;

					foreach (var result in results)
						switch (result.ResultAction)
						{
							case ResultType.Navigation:
								{
									if (result.Arguments is INavigationArgs)
									{
										var args = result.Arguments as INavigationArgs;

										// Determine Stack Change
										var options = new StackOptions() { StackChoice = args.StackType, Args = args.Parameter, ArgsKey = Convert.ToString(args.Key), NewInstance = args.NewInstance };
										StackResult stackResult = StackResult.Skipped;

										if (args.StackType != null)
											stackResult = args.ContainerId != null ? NavigationService.Navigate(args.ContainerId, args.RegionId, options) : NavigationService.Navigate(options: options);

										if (!stackResult.HasFlag(StackResult.ArgsPassed) || stackResult.HasFlag(StackResult.Skipped))
											// Determine View Load
											await NavigationService.Navigate(Convert.ToString(args.Key), args.Parameter, args.NewInstance, args.PopSource);

									}
									else if (result.Arguments is IBackNavigationArgs)
									{
										var args = result.Arguments as IBackNavigationArgs;

										if (args.Parameter == null)
											await NavigationService.GoBack();
										else
											await NavigationService.GoBack(args.Parameter);
									}

									break;
								}
							case ResultType.Error:
								await ErrorHandlingService.HandleError(result.Arguments as Exception);
								break;
							case ResultType.Display:
								var displayArgs = result.Arguments as IDisplayArgs;
								await DisplayService.ShowDialog(displayArgs.Title ?? "Dialog", displayArgs.Message);
								break;
								//TODO: Look to make useful or remove
								// Unlikely anymore uses this.
							//case ResultType.PropertyUpdate:
							//	var propertyArg = result.Arguments as IPropertyArgs;
							//	if (propertyArg == null)
							//		break;

							//	try
							//	{
							//		var propertyInfo = this.GetType().GetRuntimeProperty(propertyArg.Name);
							//		propertyInfo.SetValue(this, propertyArg.Value);
							//	}
							//	catch (Exception ex)
							//	{
							//		await ErrorHandlingService.HandleError(ex);
							//		await DisplayService.ShowDialog("Error", $"Unable to update property {propertyArg.Name}");
							//	}

							//	break;
						}
				};
			}
		}
	}
}
