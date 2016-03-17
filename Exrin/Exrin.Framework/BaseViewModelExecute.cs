using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class BaseViewModelExecute: IViewModelExecute
    {
        
        public List<IOperation> Operations { get; private set; } = new List<IOperation>();

        public int TimeoutMilliseconds { get; protected set; } = 0;

    }
}
