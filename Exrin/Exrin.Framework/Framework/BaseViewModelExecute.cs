namespace Exrin.Framework
{
    using Abstraction;
    using System.Collections.Generic;

    public class BaseViewModelExecute: IViewModelExecute
    {

        public BaseViewModelExecute() { }

        public BaseViewModelExecute(List<IBaseOperation> operations) { Operations = operations; }

        public List<IBaseOperation> Operations { get; private set; } = new List<IBaseOperation>();

        public int TimeoutMilliseconds { get; protected set; } = 60000;

    }
}
