using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IOperation<T>
    {
        /// <summary>
        /// The function or operation to be performed inside the execute
        /// </summary>
        Func<CancellationToken, Task<T>> Function { get; }
        /// <summary>
        /// If the Function fails, this function performs a rollback if necessary
        /// </summary>
        Func<Task<T>> Rollback { get; }

    }
}
