using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IModelExecution
    {
        
        /// <summary>
        /// Notify the user and handle any exception that was not expected
        /// </summary>
        /// <returns></returns>
        Func<Exception, Task<bool>> HandleUnhandledException { get; set; }

        /// <summary>
        /// If the operations are taking too long, they will be cancelled and a rollback will occur. Notify the user and handle anything associated with an operation timeout.
        /// </summary>
        /// <returns></returns>
        Func<ITimeoutEvent, Task> HandleTimeout { get; set; }

        /// <summary>
        /// A reference to the Insights object to allow the framework to record any events
        /// </summary>
        IApplicationInsights Insights { get; }

    }
}
