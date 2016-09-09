namespace Exrin.Framework
{
    using Abstraction;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    public partial class Process
    {
        
        public async static Task<T> ModelExecute<T>(this IModelExecution sender, IModelExecute<T> execute, [CallerMemberName] string name = "")
        {

            return await ModelExecute(sender,
                    operation: execute.Operation,
                    handleUnhandledException: sender.HandleUnhandledException,
                    handleTimeout: sender.HandleTimeout,
                    insights: sender.Insights,
                    name: name,
                    timeoutMilliseconds: execute.TimeoutMilliseconds
                    );

        }

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
                Func<ITimeoutEvent, Task> handleTimeout = null,
                int timeoutMilliseconds = 0,
                IApplicationInsights insights = null,
                string name = ""
                )
        {

            if (sender == null)
                throw new Exception($"The IModelExecution sender can not be null");

            // Debug Remove Timeout
            if (App.IsDebugging)
                timeoutMilliseconds = 0;

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

            var exceptionState = new PropertyArgs() { Value = false };

            if (timeoutMilliseconds > 0)
            {
                if (handleTimeout != null)
                    task.Token.Register(async (state) => { if (!(bool)((PropertyArgs)state).Value) await handleTimeout(new TimeoutEvent("", timeoutMilliseconds)); }, exceptionState);
                else if (handleUnhandledException != null)
                    task.Token.Register(async (state) => { if (!(bool)((PropertyArgs)state).Value) await handleUnhandledException(new TimeoutException()); }, exceptionState);
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
                    try
                    {
                        await Task.Run(async () => { result = await operation.Function(task.Token); }, task.Token); // Background Thread
                    }
                    catch
                    {
                        exceptionState.Value = true; // Stops registered cancel function from running, since this is exception not timeout              
                        task?.Cancel(); // Cancel all tasks
                        throw; // Go to unhandled exception
                    }
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
                    task?.Dispose();

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
