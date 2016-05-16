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

        public Model(IExrinContainer exrinContainer, IModelState modelState)
        {
            if (exrinContainer == null)
                throw new ArgumentNullException(nameof(IExrinContainer));

            _displayService = exrinContainer.DisplayService;
            _errorHandlingService = exrinContainer.ErrorHandlingService;
            _applicationInsights = exrinContainer.ApplicationInsights;

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
                    await _displayService.ShowDialog("Timeout", timeoutEvent.Message ?? "A timeout has occurred");
                };
            }
        }


    }
}
