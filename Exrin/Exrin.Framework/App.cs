using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public static class App
    {
        /// <summary>
        /// Will initialize anything needed within the framework.
        /// </summary>
        /// <param name="uiContext">Pass through SynchronizationContext.Current</param>
        public static void Init(SynchronizationContext uiContext)
        {
            ThreadHelper.Init(uiContext);
        }

    }
}
