using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    /// <summary>
    /// The result of the Execute function and what should be performed next
    /// </summary>
    public enum ResultType
    {
        None = 0,
        Navigation = 1,
        Display = 2,
        Error = 4
    }
}
