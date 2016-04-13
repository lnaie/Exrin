using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class Model : BindableModel, IModel
    {
        private IDisplayService _displayService = null;
        private IErrorHandlingService _errorHandlingService = null;
        private IApplicationInsights _applicationInsights = null;

        public IModelExecution Execution { get; protected set; }

        public Model(IDisplayService displayService, IApplicationInsights applicationInsights, IErrorHandlingService errorHandlingService, IModelState modelState)
        {
            _displayService = displayService;
            _errorHandlingService = errorHandlingService;
            _applicationInsights = applicationInsights;

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
                    await _applicationInsights.TrackException(exception);
                    await _errorHandlingService.HandleError(exception);

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
                    await _applicationInsights.TrackMetric(nameof(Metric.Timeout), timeoutEvent.MethodName); 
                    await _displayService.ShowDialog(timeoutEvent.Message ?? "A timeout has occurred");
                };
            }
        }


    }
}
