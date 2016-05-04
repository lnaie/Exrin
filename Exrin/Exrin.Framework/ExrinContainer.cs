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
		public IApplicationInsights ApplicationInsights { get; set; }

		public IDisplayService DisplayService { get; set; }

		public IErrorHandlingService ErrorHandlingService { get; set; }

		public INavigationService NavigationService { get; set; }

		public IStackRunner StackRunner { get; set; }
	}
}
