namespace Exrin.Common
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public static class ThreadHelper
    {
        private static SynchronizationContext _uiContext = null;
        public static void Init(SynchronizationContext uiContext)
        {
            _uiContext = uiContext;
        }
       
        /// <summary>
        /// Determines if the current synchronization context is the UI Thread
        /// </summary>
        /// <param name="context">SynchronizationContext you want to compare with the UIThread SynchronizationContext</param>
        /// <returns>true or false</returns>
        public static bool IsOnUIThread(SynchronizationContext context)
        {
            return context == _uiContext;
        }

        /// <summary>
        /// Will run the Func on the UIThread asynchronously.
        /// PERFORMANCE: Do not use in a loop. Context switching can cause significant performance degradation. Call this as infrequently as possible.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task RunOnUIThreadAsync(Func<Task> action)
        {
			if (_uiContext == null)
				throw new Exception("You must call Exrin.Framework.App.Init() before calling this method.");

			if (SynchronizationContext.Current == _uiContext)
				await action();
			else
				await RunOnUIThreadHelper(action);
		}

        /// <summary>
        /// Will run the Action on the UI Thread.
        /// PERFORMANCE: Do not use in a loop. Context switching can cause significant performance degradation. Call this as infrequently as possible.
        /// </summary>
        /// <param name="action"></param>
        public static void RunOnUIThread(Action action)
        {
            if (_uiContext == null)
                throw new Exception("You must call Exrin.Framework.App.Init() before calling this method.");

            if (SynchronizationContext.Current == _uiContext)
                action();
            else
                RunOnUIThreadHelper(action).Wait(); // I can wait because I am not on the same thread.
        }

        /// <summary>
        /// Will run the Func on the UI Thread.
        /// PERFORMANCE: Do not use in a loop. Context switching can cause significant performance degradation. Call this as infrequently as possible.
        /// WARNING: If on the UI Thread I can not wait for its completion before returning. Will run without waiting for its completion.
        /// </summary>
        /// <param name="action"></param>
        public static void RunOnUIThread(Func<Task> action)
        {
            if (_uiContext == null)
                throw new Exception("You must call Exrin.Framework.App.Init() before calling this method.");

            if (SynchronizationContext.Current == _uiContext)
            {
                //TODO: Test this code, still trying to find a way to run a Task synchronously.
                //Task.Factory.StartNew(async () => await action()).RunSynchronously();
                //ConfigureAwait(False)
                action(); // WARNING: If on the UI Thread I can not wait for its completion before returning.
            }
            else
                RunOnUIThreadHelper(action).Wait(); // I can wait because I am not on the same thread.
        }

        private static Task RunOnUIThreadHelper(Action action)
        {           
            var tcs = new TaskCompletionSource<bool>();

            _uiContext.Post((e) =>
            {
                try
                {
                    action();
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }, null);

            return tcs.Task;
        }

        private static Task RunOnUIThreadHelper(Func<Task> action)
        {
            var tcs = new TaskCompletionSource<bool>();

            _uiContext.Post((e) =>
            {
                try
                {
                    action().ContinueWith(t => {
                        tcs.SetResult(true);
                    });

                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }, null);

            return tcs.Task;
        }
    }

}
