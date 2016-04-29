using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exrin.Common
{
    public static class ThreadHelper
    {
        private static SynchronizationContext _uiContext = null;
        public static void Init(SynchronizationContext uiContext)
        {
            _uiContext = uiContext;
        }
       
        public static bool IsOnUIThread(SynchronizationContext context)
        {
            return context == _uiContext;
        }

        public static async Task RunOnUIThreadAsync(Action action)
        {
            await RunOnUIThreadHelper(action);
        }

        public static void RunOnUIThread(Action action)
        {
            if (_uiContext == null)
                throw new Exception("You must call Exrin.Framework.App.Init() before calling this method.");

            if (SynchronizationContext.Current == _uiContext)
                action();
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
    }

}
