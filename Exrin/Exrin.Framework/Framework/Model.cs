namespace Exrin.Framework
{
	using Abstraction;
	using System;
	using System.Threading.Tasks;

	public class Model : BindableModel, IModel, IComposition
	{
		[Obsolete("Please use ErrorHandlingService. Obsolete as of version 2.0.0.")]
		protected IErrorHandlingService _errorHandlingService => ErrorHandlingService;
		[Obsolete("Please use ApplicationInsights. Obsolete as of version 2.0.0.")]
		protected IApplicationInsights _applicationInsights => ApplicationInsights;
		[Obsolete("Please use DisplayService. Obsolete as of version 2.0.0.")]
		protected IDisplayService _displayService => DisplayService;

		protected IErrorHandlingService ErrorHandlingService { get; private set; }
		protected IApplicationInsights ApplicationInsights { get; private set; }
		protected IDisplayService DisplayService { get; private set; }

		public IModelExecution Execution { get; protected set; }

		public Model()
		{
			Execution = new ModelExecution()
			{
				HandleTimeout = TimeoutHandle,
				HandleUnhandledException = HandleError
			};
		}

		void IComposition.SetContainer(IExrinContainer exrinContainer)
		{
			if (_containerSet)
				return;

			if (exrinContainer == null)
				throw new ArgumentNullException(nameof(IExrinContainer));

			ApplicationInsights = exrinContainer.ApplicationInsights;
			DisplayService = exrinContainer.DisplayService;
			ErrorHandlingService = exrinContainer.ErrorHandlingService;

			_containerSet = true;
		}

		public bool _containerSet = false;

		public Model(IModelState modelState)
		{
			Execution = new ModelExecution()
			{
				HandleTimeout = TimeoutHandle,
				HandleUnhandledException = HandleError
			};

			ModelState = modelState;
		}

		public Model(IExrinContainer exrinContainer, IModelState modelState)
		{
			if (exrinContainer == null)
				throw new ArgumentNullException(nameof(IExrinContainer));

			DisplayService = exrinContainer.DisplayService;
			ErrorHandlingService = exrinContainer.ErrorHandlingService;
			ApplicationInsights = exrinContainer.ApplicationInsights;

			_containerSet = true;

			ModelState = modelState;

			Execution = new ModelExecution()
			{
				HandleTimeout = TimeoutHandle,
				HandleUnhandledException = HandleError
			};
		}

		public IModelState ModelState { get; set; }

		private Func<Exception, Task<bool>> HandleError
		{
			get
			{
				return async (exception) =>
				{
					await ApplicationInsights.TrackException(exception);
					await ErrorHandlingService.HandleError(exception);

					return true;
				};
			}
		}

		private Func<ITimeoutEvent, Task> TimeoutHandle
		{
			get
			{
				return async (timeoutEvent) =>
				{
					await ApplicationInsights.TrackMetric(nameof(Metric.WebLoadTimeout), timeoutEvent.MethodName);
					await DisplayService.ShowDialog("Timeout", timeoutEvent.Message ?? "A timeout has occurred");
				};
			}
		}


	}
}
