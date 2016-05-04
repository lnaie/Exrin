using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
	public interface IExrinContainer
	{
		IApplicationInsights ApplicationInsights { get; set; }
		IDisplayService DisplayService { get; set; }
		INavigationService NavigationService { get; set; }
		IErrorHandlingService ErrorHandlingService { get; set; }
		IStackRunner StackRunner { get; set; }
	}
}
