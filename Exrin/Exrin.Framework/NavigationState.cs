using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class NavigationState : INavigationReadOnlyState
    {
        public string ViewName { get; set; }
    }
}
