using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;
using Exrin.Common;
using Exrin.Framework;

namespace Exrin.Framework
{
	public abstract class ViewModel : BindableModel, IViewModel
	{
		protected IExecution Execution { get; set; }
		protected readonly IDisplayService _displayService = null;
		protected readonly INavigationService _navigationService = null;
		protected readonly IErrorHandlingService _errorHandlingService = null;
		protected readonly IApplicationInsights _applicationInsights = null;
		protected readonly IStackRunner _stackRunner = null;

		public ViewModel(IExrinContainer exrinContainer, IVisualState visualState, [CallerFilePath] string caller = nameof(ViewModel))

		{

            if (exrinContainer == null)
                throw new ArgumentNullException(nameof(IExrinContainer));

			_applicationInsights = exrinContainer.ApplicationInsights;
			_displayService = exrinContainer.DisplayService;
			_navigationService = exrinContainer.NavigationService;
			_errorHandlingService = exrinContainer.ErrorHandlingService;
			_stackRunner = exrinContainer.StackRunner;

			VisualState = visualState;

			if (VisualState != null)
				Task.Run(() => visualState.Init())
					.ContinueWith((task) =>
					{
						if (task.Exception != null)
							_applicationInsights.TrackException(task.Exception);
					});

			Execution = new Execution()
			{
				HandleTimeout = TimeoutHandle,
				NotifyOfActivity = NotifyActivity,
				NotifyActivityFinished = NotifyActivityFinished,
				HandleResult = HandleResult
			};

		}

        public VisualStatus ViewStatus { get; private set; } = VisualStatus.Unseen;

		public IVisualState VisualState { get; set; }

		private IDictionary<string, IRelayCommand> commands = new Dictionary<string, IRelayCommand>();
		public IRelayCommand GetCommand(Func<IRelayCommand> create, [CallerMemberName] string name = "")
		{
			if (!commands.ContainsKey(name))
				commands.Add(name, create());

			return commands[name];
		}

		private bool _isBusy = false;
		public bool IsBusy { get { return _isBusy; } set { _isBusy = value; OnPropertyChanged(); } }

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

		public virtual void OnPopped() { ViewStatus = VisualStatus.Disposed; }
        
		protected Func<Task> TimeoutHandle
		{
			get
			{
				return async () =>
				{
					await _displayService.ShowDialog("Timeout", "Operation failed to complete within an acceptable amount of time");
				};
			}
		}

		protected Func<Task> NotifyActivity
		{
			get
			{
				return () =>
				{

					IsBusy = true;

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
					IsBusy = false;

					return Task.FromResult(0);
				};
			}
		}

		protected Func<IList<IResult>, Task> HandleResult
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
									var args = result.Arguments as INavigationArgs;

									// Determine Stack Change
									_stackRunner.Run(args.StackType);

									// Determine View Load
									await _navigationService.Navigate(Convert.ToString(args.Key), args.Parameter);

									break;
								}
							case ResultType.Error:
								await _errorHandlingService.HandleError(result.Arguments as Exception);
								break;
							case ResultType.Display:
								var displayArgs = result.Arguments as IDisplayArgs;
								await _displayService.ShowDialog(displayArgs.Title, displayArgs.Message);
								break;
							case ResultType.PropertyUpdate:
								var propertyArg = result.Arguments as IPropertyArgs;
								if (propertyArg == null)
									break;

								try
								{
									var propertyInfo = this.GetType().GetRuntimeProperty(propertyArg.Name);
									propertyInfo.SetValue(this, propertyArg.Value);
								}
								catch (Exception ex)
								{
									await _errorHandlingService.HandleError(ex);
									await _displayService.ShowDialog("Error", $"Unable to update property {propertyArg.Name}");
								}

								break;
						}

				};
			}
		}
	}
}
