using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class ModelExecution : IModelExecution
    {
        public Func<Task> HandleTimeout { get; set; } = () => { return Task.FromResult(0); };

        public Func<Exception, Task<bool>> HandleUnhandledException { get; set; } = (exception) => { return Task.FromResult(true); };

        public IApplicationInsights Insights { get;}
    }
}
