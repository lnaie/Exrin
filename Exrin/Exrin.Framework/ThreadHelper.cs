using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public static class ThreadHelper
    {

        public static void RunOnUIThread(SynchronizationContext uiContext, Action action)
        {
            // TODO: Initial the context when initializing the framework
            // Then no need to pass it through.

            // SynchronizationContext.Current

            uiContext.Post((e) => action(), null);
        }

    }
}
