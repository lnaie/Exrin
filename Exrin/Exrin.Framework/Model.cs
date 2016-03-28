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

        public Model(IDisplayService displayService, IErrorHandlingService errorHandlingService)
        {
            _displayService = displayService;
            _errorHandlingService = errorHandlingService;

            Execution = new ModelExecution()
            {
                HandleTimeout = TimeoutHandle,
                HandleUnhandledException = HandleError
            };
        }

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
