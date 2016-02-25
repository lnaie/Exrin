using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework.Tests.ViewModelExecute.Objects
{
    public class TestOperation : IOperation
    {
        public bool ChainedRollback { get; } = false;

        public Func<IResult, Task> Function { get
            {
                return async (result) =>
                {
                    // Execute Code Here

                    await Task.Delay(200);
                };

            }
        } 

        public Func<IResult, Task> Rollback { get; }
    }
}
