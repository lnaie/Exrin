using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    private static async Task ViewModelExecute(IExecutionComplete sender,
             List<IOperation> operations,
             Func<Task> notifyOfActivity = null,
             Func<Task> notifyActivityFinished = null,
             Func<Task<bool>> handleUnhandledException = null,
             Func<Task> handleTimeout = null,
             int timeoutMilliseconds = 0,
             Func<Task> completed = null,
             IApplicationInsights insights = null,
             string name = ""
             )
    {
        // If current executing, ignore the latest request
        lock (sender)
        {
            if (!_status.ContainsKey(sender))
                _status.Add(sender, true);
            else if (_status[sender])
                return;
            else
                _status[sender] = true;
        }

        try
        {
            if (insights != null)
                insights.TrackEvent(name, $"User activated {name}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"insights.TrackEvent({name}) {ex.Message}");
        }

        List<Func<Task>> rollbacks = new List<Func<Task>>();
        bool transactionRunning = false;

        // Setup Cancellation of Tasks if long running
        var task = new CancellationTokenSource();

        if (timeoutMilliseconds > 0)
        {
            if (handleTimeout != null)
                task.Token.Register(async () => { await handleTimeout(); });
            else if (handleUnhandledException != null)
                task.Token.Register(async () => { await handleUnhandledException(); });
            else
                throw new Exception($"You must specify either {nameof(handleTimeout)} or {nameof(handleUnhandledException)} to handle a timeout.");

            task.CancelAfter(timeoutMilliseconds);
        }

        try
        {
            if (notifyOfActivity == null)
                throw new Exception($"{nameof(notifyOfActivity)} is null: You must notify the user that something is happening");

            await notifyOfActivity();

            // Transaction Block
            transactionRunning = true;

            foreach (var operation in operations)
            {
                rollbacks.Add(operation.Rollback);

                if (operation.Function != null)
                    await await RunOnBackgroundThreadAsync(operation.Function, task.Token); // But wait for it to finish

                if (!operation.ChainedRollback)
                    rollbacks.Remove(operation.Rollback);
            }

            rollbacks.Clear();
            transactionRunning = false;
            // End of Transaction Block

            if (notifyActivityFinished == null)
                throw new Exception($"{nameof(notifyActivityFinished)} is null: You need to specify what happens when the operations finish");

            await notifyActivityFinished();

            if (completed != null)
                await completed();
        }
        catch
        {
            if (handleUnhandledException == null)
                throw;

            var handled = await handleUnhandledException();
            if (!handled)
                throw;
        }
        finally
        {
            try
            {
                if (transactionRunning)
                {
                    rollbacks.Reverse(); // Do rollbacks in reverse order
                    foreach (var rollback in rollbacks)
                        await rollback();
                }

                await sender.HandleResult();
            }
            finally
            {
                _status.Remove(sender);
            }

        }
    }
}
