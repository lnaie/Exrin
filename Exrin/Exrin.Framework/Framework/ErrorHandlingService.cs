namespace Exrin.Framework
{
    using Abstraction;
    using System;
    using System.Threading.Tasks;

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
