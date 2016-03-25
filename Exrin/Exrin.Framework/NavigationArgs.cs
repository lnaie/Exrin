using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class NavigationArgs: IResultArgs
    {

        public object StackType { get; set; }

        public object PageIndicator { get; set; }

        public object Parameter { get; set; }

    }
}
