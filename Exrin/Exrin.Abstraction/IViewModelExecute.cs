using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    /// <summary>
    /// An execution entity to perform when an action is taken in the ViewModel
    /// </summary>
    public interface IViewModelExecute
    {
        /// <summary>
        /// A list of operations to perform in sequence and their associated rollbacks
        /// </summary>
        List<IOperation> Operations { get; set; }
        /// <summary>
        /// To notify the user of the app that the operations are in progress
        /// </summary>
        /// <returns></returns>
        Task NotifyOfActivity();
        /// <summary>
        /// To notify the user of the app that the operations have now finished or failed
        /// </summary>
        /// <returns></returns>
        Task NotifyActivityFinished();
        /// <summary>
        /// Notify the user and handle any exception that was not expected
        /// </summary>
        /// <returns></returns>
        Task<bool> HandleUnhandledException(Exception e);
        /// <summary>
        /// If the operations are taking too long, they will be cancelled and a rollback will occur. Notify the user and handle anything associated with an operation timeout.
        /// </summary>
        /// <returns></returns>
        Task HandleTimeout();
    
        /// <summary>
        /// A reference to the Insights object to allow the framework to record any events
        /// </summary>
        IApplicationInsights Insights { get; }
        /// <summary>
        /// The total length allowed for the operations to complete, before a timeout is triggered. Setting to 0 or below will result in no timeout constraint.
        /// </summary>
        int TimeoutMilliseconds { get; set; }

    }
}
