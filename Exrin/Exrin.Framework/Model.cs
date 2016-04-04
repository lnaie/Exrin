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

        public IModelExecution Execution { get; protected set; }

        public Model(IDisplayService displayService, IErrorHandlingService errorHandlingService, IModelState modelState)
        {
            _displayService = displayService;
            _errorHandlingService = errorHandlingService;
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
                    await _errorHandlingService.ReportError(exception);

                    return true;
                };
            }

        }
        
        private Func<Task> TimeoutHandle
        {
            get
            {
                return async () =>
                {
                    await _displayService.ShowDialog("A timeout has occurred");
                };
            }
        }


    }
}
