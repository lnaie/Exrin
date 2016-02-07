using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IExecutionResult
    {
        ResultType ResultAction { get; set; }
        object Arguments { get; set; }
    }
}
