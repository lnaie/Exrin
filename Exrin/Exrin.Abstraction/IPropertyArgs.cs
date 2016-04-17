using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IPropertyArgs: IResultArgs
    {
        string Name { get; set; }

        object Value { get; set; }
    }
}
