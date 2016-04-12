using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IViewNavigationArgs
    {
        IView PoppedView { get; set; }

        IView CurrentView { get; set; } 
    }
}
