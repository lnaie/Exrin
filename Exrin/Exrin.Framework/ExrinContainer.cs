using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
	public class ExrinContainer : IExrinContainer
	{

        public ExrinContainer(IApplicationInsights applicationInsights,
                              IDisplayService displayService,
                              IErrorHandlingService errorHandlingService,
                              INavigationService navigationService,
                              IStackRunner stackRunner)
        {
            ApplicationInsights = applicationInsights;
            DisplayService = displayService;
            ErrorHandlingService = errorHandlingService;
            NavigationService = navigationService;
            StackRunner = stackRunner;
        }

		public IApplicationInsights ApplicationInsights { get; private set; }

		public IDisplayService DisplayService { get; private set; }

		public IErrorHandlingService ErrorHandlingService { get; private set; }

		public INavigationService NavigationService { get; private set; }

		public IStackRunner StackRunner { get; private set; }
	}
}
