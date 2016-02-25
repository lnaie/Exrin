using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework.Tests.Builder
{
    public class ExecutionBuilder
    {

        public IExecution BuildNew()
        {
            return new Execution();
        }

    }
}
