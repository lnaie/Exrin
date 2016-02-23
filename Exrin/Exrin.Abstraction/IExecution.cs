using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    /// <summary>
    /// To be given to an execution instance on each View Model
    /// </summary>
    public interface IExecution
    {
        /// <summary>
        /// The end result of all operations
        /// </summary>
        IResult Result { get; set; }
        /// <summary>
        /// The method that will handle the end result once all operations are complete
        /// </summary>
        /// <returns></returns>
        Func<IResult, Task> HandleResult { get; set; }
        /// <summary>
        /// To notify the user of the app that the operations are in progress
        /// </summary>
        /// <returns></returns>
        Func<Task> NotifyOfActivity { get; set; }
        /// <summary>
        /// To notify the user of the app that the operations have now finished or failed
        /// </summary>
        /// <returns></returns>
        Func<Task> NotifyActivityFinished { get; set; }
        /// <summary>
        /// Notify the user and handle any exception that was not expected
        /// </summary>
        /// <returns></returns>
        Func<Exception,Task<bool>> HandleUnhandledException { get; set; }
        /// <summary>
        /// If the operations are taking too long, they will be cancelled and a rollback will occur. Notify the user and handle anything associated with an operation timeout.
        /// </summary>
        /// <returns></returns>
        Func<Task> HandleTimeout { get; set; }

        /// <summary>
        /// A reference to the Insights object to allow the framework to record any events
        /// </summary>
        IApplicationInsights Insights { get; }
    }
}
