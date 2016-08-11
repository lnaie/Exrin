namespace Exrin.Framework
{
    using Abstraction;

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
