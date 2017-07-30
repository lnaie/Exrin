namespace Exrin.Framework
{
    using Abstraction;
    using System;
    using System.Threading.Tasks;

    public class ModelExecution : IModelExecution
    {

        public Func<ITimeoutEvent, Task> HandleTimeout { get; set; } = (timeoutEvent) => { return Task.FromResult(0); };

        public Func<Exception, Task<bool>> HandleUnhandledException { get; set; } = (exception) => { return Task.FromResult(true); };

        public IApplicationInsights Insights { get; set; }
    }
}
