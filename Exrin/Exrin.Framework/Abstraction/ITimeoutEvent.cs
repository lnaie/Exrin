using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface ITimeoutEvent: IEvent
    {
        string MethodName { get; }
        int TimeoutMilliseconds { get; }
        string Message { get; }
    }
}
