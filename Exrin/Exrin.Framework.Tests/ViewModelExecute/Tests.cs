﻿using Exrin.Abstraction;
using Exrin.Framework.Tests.Builder;
using Exrin.Framework.Tests.Helper;
using Exrin.Framework.Tests.ViewModelExecute.Objects;
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
        bool IsBusy { get; set; }

        public async Task Test()
        {
            // Interface and Project based bindings
           
            // Dialog, Navigation Service, 

            var builder = new ExecutionBuilder();

            INavigationService navigationService = new NavigationService();
            IErrorHandlingService errorHandlingService = new ErrorHandlingService();
            IDisplayService displayService = new DisplayService();

            Handler handler = new Handler(navigationService, errorHandlingService, displayService);
            
            // Should be built top level view model
            var execution = builder.BuildNew(handler);

            // Need easier way to add and build operations to ViewModel
            var operation = new TestViewModelExecute()
            {
                Operations = new List<IOperation>() { new TestOperation() },
                TimeoutMilliseconds = 10000
            };

            await execution.ViewModelExecute(operation);

        }


        // Values over 1000 but only slight can possibly fail due to minor inaccuracy in the timer function. 
        // This is deemed acceptable, with a 50ms variance.
        //[Theory]
        //[InlineData(10)]
        //[InlineData(500)]
        //[InlineData(999)]
        //[InlineData(1050)]
        //public async Task TimeoutHandledUnder1000ms(int timeout)
        //{

        //    var timeoutHandled = false;

        //    Func<Task> timeoutHandle = async () => { timeoutHandled = true; };
        //    Func<Task> waitFunction = async () => { await Task.Delay(1000); };
        //    Func<Task> notifyActivity = async () => { };
        //    Func<Task> notifyActivityFinished = async () => { };
        //    Func<Task> completed = async () => { };

        //    await new Object().ViewModelExecute(operations: new List<IOperation>() {
        //                                                             new Operation() { Function = waitFunction, Rollback = null }
        //                                                                               },
        //                                             handleTimeout: timeoutHandle,
        //                                             timeoutMilliseconds: timeout,
        //                                             notifyOfActivity: notifyActivity,
        //                                             notifyActivityFinished: notifyActivityFinished,
        //                                             completed: completed
        //                                            );

        //    if (timeout <= 1000)
        //        Assert.Equal(true, timeoutHandled);
        //    else
        //        Assert.Equal(false, timeoutHandled);

        //}


    }
}