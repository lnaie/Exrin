namespace Exrin.Framework
{
    using Abstraction;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Execution : IExecution
    {
        public IApplicationInsights Insights { get; private set; }

        public IList<IResult> Result { get; set; }

        public Execution() { }

        public Func<IList<IResult>, Task> HandleResult { get; set; }

        public Func<Task> HandleTimeout { get; set; }

        public Func<Exception, Task<bool>> HandleUnhandledException { get; set; }

        public Func<Task> NotifyActivityFinished { get; set; }

        public Func<Task> NotifyOfActivity { get; set; }

		public Func<object, Task<bool>> PreCheck { get; set; }
	}
}