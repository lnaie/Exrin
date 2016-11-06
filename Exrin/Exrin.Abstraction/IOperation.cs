namespace Exrin.Abstraction
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IChainedOperation : IOperation { }
    
    /// <summary>
    /// An operation and associated rollback to be performed inside an Execute
    /// </summary>
    [Obsolete("Use IChainedOperation or ISingleOperation")]
    public interface IOperation : IBaseOperation
    {
        /// <summary>
        /// The function or operation to be performed inside the execute
        /// </summary>
        Func<IList<IResult>, object, CancellationToken, Task> Function { get; }

    }
}
