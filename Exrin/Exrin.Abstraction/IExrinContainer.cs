using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
	public interface IExrinContainer
	{
		IApplicationInsights ApplicationInsights { get; }
		IDisplayService DisplayService { get; }
		INavigationService NavigationService { get; }
		IErrorHandlingService ErrorHandlingService { get; }
		IStackRunner StackRunner { get; }
	}
}
