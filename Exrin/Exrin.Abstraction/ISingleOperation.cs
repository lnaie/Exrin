namespace Exrin.Abstraction
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ISingleOperation: IBaseOperation
    {
        /// <summary>
        /// The function or operation to be performed inside the execute
        /// </summary>
        Func<object, CancellationToken, Task<IList<IResult>>> Function { get; }
    }
}
