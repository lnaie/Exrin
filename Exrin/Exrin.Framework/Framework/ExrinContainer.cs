namespace Exrin.Framework
{
    using Abstraction;

    public class ExrinContainer : IExrinContainer
	{

        public ExrinContainer(IApplicationInsights applicationInsights,
                              IDisplayService displayService,
                              IErrorHandlingService errorHandlingService,
                              INavigationService navigationService)
        {
            ApplicationInsights = applicationInsights;
            DisplayService = displayService;
            ErrorHandlingService = errorHandlingService;
            NavigationService = navigationService;
        }

		public IApplicationInsights ApplicationInsights { get; private set; }

		public IDisplayService DisplayService { get; private set; }

		public IErrorHandlingService ErrorHandlingService { get; private set; }

		public INavigationService NavigationService { get; private set; }
	}
}
