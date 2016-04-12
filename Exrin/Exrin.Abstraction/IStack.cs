using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IStack
    {
        object StackIdentifier { get; set; }
        StackStatus Status { get; set; }
        INavigationContainer Container { get; }
        Task StartNavigation(object args = null);
        void Init();

    }
}
