using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IMultiView: IView
    {
        IList<IView> Views { get; } 
    }
}
