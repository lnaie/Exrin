using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class Handler
    {

        INavigationService _navigationService = null;
        IErrorHandlingService _errorHandlingService = null;
        IDisplayService _displayService = null;


        public Handler(INavigationService navigationService, IErrorHandlingService errorHandlingService, IDisplayService displayService)
        {
            _navigationService = navigationService;
            _errorHandlingService = errorHandlingService;
            _displayService = displayService;

        }

        public Func<Task> HandleTimeout()
        {
            return async () =>
                {
                    await _displayService.ShowDialog("This action took too long. Please try again.");
                };
        }

        public Func<IResult, Task> HandleResult()
        {
            return async (result) =>
                {
                    switch (result.ResultAction)
                    {
                        case ResultType.Navigation:
                            await _navigationService.Navigate(Convert.ToString(result.Arguments));
                            break;
                        case ResultType.Error:
                            await _errorHandlingService.ReportError(result.Arguments as Exception);
                            break;
                        case ResultType.Display:
                            await _displayService.ShowDialog(result.Arguments as string);
                            break;
                    }

                };
        }

    }
}
