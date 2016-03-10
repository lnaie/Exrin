using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class Execution : IExecution
    {
        public IApplicationInsights Insights { get; private set; }

        public IResult Result { get; set; }
        
        public Execution()
        {
        }

        public Func<IResult, Task> HandleResult { get; set; }

        public Func<Task> HandleTimeout { get; set; }

        public Func<Exception, Task<bool>> HandleUnhandledException { get; set; }

        public Func<Task> NotifyActivityFinished { get; set; }

        public Func<Task> NotifyOfActivity { get; set; }

    }
}
