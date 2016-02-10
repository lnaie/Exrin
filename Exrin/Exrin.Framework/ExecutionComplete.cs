using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class ExecutionComplete : IExecutionComplete
    {
        public IExecutionResult Result
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Task HandleResult()
        {
            throw new NotImplementedException();
        }
    }
}
