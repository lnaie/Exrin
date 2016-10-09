namespace Exrin.Framework
{
    using Abstraction;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class Operation : IOperation
    {
        public bool ChainedRollback { get; set; } = true;

        public Func<IList<IResult>, object, CancellationToken, Task> Function { get; set; } = null;

        public Func<Task> Rollback { get; set; } = null;
    }

    public class Operation<T> : IOperation<T>
    {

        public Func<CancellationToken, Task<T>> Function { get; set; } = null;

        public Func<Task<T>> Rollback { get; set; } = null;
      
    }
}
