namespace Exrin.Insights
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class Timer : CancellationTokenSource
    {
        internal Timer(Action<object> callback, object state, int millisecondsDueTime, int millisecondsPeriod, bool waitForCallbackBeforeNextPeriod = false)
        {
            Task.Delay(millisecondsDueTime, Token)
                .ContinueWith(async (t, s) =>
                {
                    if (t.IsFaulted)
                    {
                        throw t.Exception;
                    }

                    var tuple = (Tuple<Action<object>, object>)s;

                    while (!IsCancellationRequested)
                    {
                        if (waitForCallbackBeforeNextPeriod)
                            tuple.Item1(tuple.Item2);
                        else
                            await Task.Factory.StartNew(() => tuple.Item1(tuple.Item2)); //TODO: Double check that this is only awaiting the Task Creation and not the execution

                        await Task.Delay(millisecondsPeriod, Token).ConfigureAwait(false);
                    }
                }, 
                Tuple.Create(callback, state), 
                CancellationToken.None, 
                TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion, 
                TaskScheduler.Default);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Cancel();

            base.Dispose(disposing);
        }
    }
}
