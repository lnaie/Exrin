using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    /// <summary>
    /// An operation and associated rollback to be performed inside an Execute
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// The function or operation to be performed inside the execute
        /// </summary>
        Func<IList<IResult>, object, CancellationToken, Task> Function { get; }
        /// <summary>
        /// If the Function fails, this function performs a rollback if necessary
        /// </summary>
        Func<Task> Rollback { get; }
        /// <summary>
        /// If a Function fails later on in the list, do you want to perform this rollback?
        /// </summary>
        bool ChainedRollback { get; }
    }
}
