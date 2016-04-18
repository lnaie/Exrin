using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface INavigationArgs: IResultArgs
    {
        object StackType { get; set; }

        object Key { get; set; }

        object Parameter { get; set; }
    }
}
