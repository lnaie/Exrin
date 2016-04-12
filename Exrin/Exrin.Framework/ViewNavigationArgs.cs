using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class ViewNavigationArgs : IViewNavigationArgs
    {
        public IView CurrentView { get; set; }

        public IView PoppedView { get; set; }

        public object Parameter { get; set; }
    }
}
