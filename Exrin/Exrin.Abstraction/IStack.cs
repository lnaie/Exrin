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
        bool ShowNavigationBar { get; set; }
        StackStatus Status { get; set; }
        INavigationContainer Container { get; }
        IMasterDetailView MasterView { get; }
        Task StartNavigation(object args = null, bool loadStartKey = true);
        void Init();

    }
}
