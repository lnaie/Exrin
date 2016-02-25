using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework.Tests.Builder
{
    public class ExecutionBuilder
    {

        public IExecution BuildNew(Handler handler)
        {
            var execution = new Execution();

            execution.HandleResult = handler.HandleResult();
            execution.HandleTimeout = handler.HandleTimeout();
            execution.HandleUnhandledException = handler.HandleUnhandledException();
            execution.NotifyActivityFinished = handler.NotifyOfActivityFinished();
            execution.NotifyOfActivity = handler.NotifyOfActivity();

            return execution;
        }

    }
}
