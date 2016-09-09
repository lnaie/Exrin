namespace Exrin.Framework
{
    using Abstraction;
    using System.Collections.Generic;

    public class BaseViewModelExecute: IViewModelExecute
    {

        public BaseViewModelExecute() { }

        public BaseViewModelExecute(List<IOperation> operations) { Operations = operations; }

        public List<IOperation> Operations { get; private set; } = new List<IOperation>();

        public int TimeoutMilliseconds { get; protected set; } = 30000;

    }
}
