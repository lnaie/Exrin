namespace Exrin.Framework
{
    using Abstraction;
    using System.Collections.Generic;

    public class BaseViewModelExecute: IViewModelExecute
    {
        
        public List<IOperation> Operations { get; private set; } = new List<IOperation>();

        public int TimeoutMilliseconds { get; protected set; } = 0;

    }
}
