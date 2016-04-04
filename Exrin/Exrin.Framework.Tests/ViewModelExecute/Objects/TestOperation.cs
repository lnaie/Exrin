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

        public Func<IList<IResult>, object, Task> Function { get
            {
                return async (result, parameter) =>
                {
                    // Execute Code Here

                    await Task.Delay(200);
                };

            }
            set { }
        } 

        public Func<Task> Rollback { get; }
    }
}
