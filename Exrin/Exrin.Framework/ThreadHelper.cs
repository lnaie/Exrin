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
        private static SynchronizationContext _uiContext = null;
        public static void Init(SynchronizationContext uiContext)
        {
            _uiContext = uiContext;
        }
        public static void RunOnUIThread(Action action)
        {           
            if (_uiContext != null)
                _uiContext.Post((e) => action(), null);
            else
                throw new Exception("You must call Exrin.Framework.App.Init() before calling this method.");
        }

    }
}
