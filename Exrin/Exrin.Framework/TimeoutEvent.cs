using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class TimeoutEvent : ITimeoutEvent
    {

        public TimeoutEvent(string methodName, int timeoutMilliseconds, string message = "")
        {
            _methodName = methodName;
            _timeoutMilliseconds = timeoutMilliseconds;
            _message = message;
        }

        private string _methodName = "";
        public string MethodName { get { return _methodName; } }

        private int _timeoutMilliseconds = -1;
        public int TimeoutMilliseconds { get { return _timeoutMilliseconds; } }

        private string _message = "";
        public string Message { get { return _message; } }
    }
}
