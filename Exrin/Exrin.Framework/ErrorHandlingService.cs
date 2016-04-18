using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class ErrorHandlingService : IErrorHandlingService
    {
        private readonly IDisplayService _displayService = null;
        public ErrorHandlingService(IDisplayService displayService)
        {
            _displayService = displayService;
        }

        public Task HandleError(Exception ex)
        {
            if (ex == null)
                _displayService.ShowDialog("Error", "Unknown Error Occurred");
            else
                _displayService.ShowDialog("Error", ex.Message);

            return Task.FromResult(true);
        }
    }
}
