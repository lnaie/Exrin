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
        public IResultArgs Arguments { get; set; }

        public ResultType ResultAction { get; set; }
    }
}
