using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class PageNavigationArgs : IPageNavigationArgs
    {
        public IPage CurrentPage { get; set; }

        public IPage PoppedPage { get; set; }

        public object Parameter { get; set; }
    }
}
