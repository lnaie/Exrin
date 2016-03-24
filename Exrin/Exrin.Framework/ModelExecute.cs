using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public partial class Process
    {

        /// <summary>
        /// Singular Operation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <param name="operation"></param>
        /// <param name="handleUnhandledException"></param>
        /// <param name="handleTimeout"></param>
        /// <param name="timeoutMilliseconds"></param>
        /// <param name="insights"></param>
        /// <param name="name"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private static async Task<T> ModelExecute<T>(IModelExecution sender,
                IOperation<T> operation,
                Func<Exception, Task<bool>> handleUnhandledException = null,
                Func<Task> handleTimeout = null,
                int timeoutMilliseconds = 0,
                IApplicationInsights insights = null,
                string name = "",
                object parameter = null
                )
        {


            if (sender == null)
                throw new Exception($"The IModelExecution sender can not be null");


            // Background thread
            Task insight = Task.Run(() =>
            {
                try
                {
                    if (insights != null)
                        insights.TrackEvent(name, $"User activated {name}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"insights.TrackEvent({name}) {ex.Message}");
                }
            });

            List<Func<IResult, Task>> rollbacks = new List<Func<IResult, Task>>();
            bool transactionRunning = false;

            // Setup Cancellation of Tasks if long running
            var task = new CancellationTokenSource();

            if (timeoutMilliseconds > 0)
            {
                if (handleTimeout != null)
                    task.Token.Register(async () => { await handleTimeout(); });
                else if (handleUnhandledException != null)
                    task.Token.Register(async () => { await handleUnhandledException(new TimeoutException()); });
                else
                    throw new Exception($"You must specify either {nameof(handleTimeout)} or {nameof(handleUnhandledException)} to handle a timeout.");

                task.CancelAfter(timeoutMilliseconds);
            }


            T result = default(T);

            try
            {

                // Transaction Block
                transactionRunning = true;

                if (operation.Function != null)
                    await Task.Run(async () => { result = await operation.Function(); }, task.Token); // Background Thread


                transactionRunning = false;
                // End of Transaction Block

               
            }
            catch (Exception e)
            {
                if (handleUnhandledException == null)
                    throw;

                var handled = await handleUnhandledException(e);
                if (!handled)
                    throw;
            }
            finally
            {
                try
                {
                    if (transactionRunning)
                        await operation.Rollback();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

            }

            return result;
        }


    }
}
