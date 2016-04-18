using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class PropertyArgs: IPropertyArgs
    {
        public string Name { get; set; }

        public object Value { get; set; }
    }
}
