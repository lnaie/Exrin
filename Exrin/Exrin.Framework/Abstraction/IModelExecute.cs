using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
   public interface IModelExecute<T>
    {

        /// <summary>
        /// The operation to perform and its rollback
        /// </summary>
        IOperation<T> Operation { get; }

        /// <summary>
        /// The total length allowed for the operations to complete, before a timeout is triggered. Setting to 0 or below will result in no timeout constraint.
        /// </summary>
        int TimeoutMilliseconds { get; }

    }
}
