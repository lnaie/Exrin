using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class Result : IResult
    {
        public Result (object parameter)
        {
            Parameter = parameter;
        }
        public object Parameter { get; private set; }

        public IResultArgs Arguments { get; set; }

        public ResultType ResultAction { get; set; }
    }
}
