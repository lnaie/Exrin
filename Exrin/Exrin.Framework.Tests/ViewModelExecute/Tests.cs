using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Exrin.Framework.Tests.ViewModelExecute
{
    public class Tests
    {


        // Values over 1000 but only slight can possibly fail due to minor inaccuracy in the timer function. 
        // This is deemed acceptable, with a 50ms variance.
        [Theory]
        [InlineData(10)]
        [InlineData(500)]
        [InlineData(999)]
        [InlineData(1050)]
        public async Task TimeoutHandledUnder1000ms(int timeout)
        {

            var timeoutHandled = false;

            Func<Task> timeoutHandle = async () => { timeoutHandled = true; };
            Func<Task> waitFunction = async () => { await Task.Delay(1000); };
            Func<Task> notifyActivity = async () => { };
            Func<Task> notifyActivityFinished = async () => { };
            Func<Task> completed = async () => { };

            await new Object().ViewModelExecute(operations: new List<IOperation>() {
                                                                     new Operation() { Function = waitFunction, Rollback = null }
                                                                                       },
                                                     handleTimeout: timeoutHandle,
                                                     timeoutMilliseconds: timeout,
                                                     notifyOfActivity: notifyActivity,
                                                     notifyActivityFinished: notifyActivityFinished,
                                                     completed: completed
                                                    );

            if (timeout <= 1000)
                Assert.Equal(true, timeoutHandled);
            else
                Assert.Equal(false, timeoutHandled);

        }


    }
}
