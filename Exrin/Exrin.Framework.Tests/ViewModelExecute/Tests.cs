using Exrin.Abstraction;
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

            // Should be built top level view model
            var execution = builder.BuildNew();

            // ** START ** All of this is BaseViewModel Code

            //TODO: See if there is a way to house the dialog service and navigation service within the framework
            // Helper Functions
            INavigationService navigationService = new NavigationService();
            IErrorHandlingService errorHandlingService = new ErrorHandlingService();
            IDisplayService displayService = new DisplayService();

            //TODO: Need to handle complex result arguments (or make sure its left open for others to modify)
            Func<IResult, Task> handleResult = async (result) =>
            {
                switch (result.ResultAction)
                {
                    case ResultType.Navigation:
                        await navigationService.Navigate(Convert.ToString(result.Arguments));
                        break;
                    case ResultType.Error:
                        await errorHandlingService.ReportError(result.Arguments as Exception);
                        break;
                    case ResultType.Display:
                        await displayService.ShowDialog(result.Arguments as string);
                        break;
                }

            };

            Func<Task> handleTimeout = async () =>
            {
                await displayService.ShowDialog("Timeout occurred");
            };

            Func<Exception, Task<bool>> handleUnhandledException = async (exception) =>
            {
                await errorHandlingService.ReportError(exception);
                await displayService.ShowDialog("Error occurred");

                return true;
            };

            Func<Task> notifyOfActivityFinished = () =>
            {
                return Task.Run(() => { IsBusy = false; });
            };

            Func<Task> notifyOfActivity = () =>
            {
                return Task.Run(() => { IsBusy = true; });
            };
            
            // ** END ** BaseViewModel Code

            execution.HandleResult = handleResult;
            execution.HandleTimeout = handleTimeout;
            execution.HandleUnhandledException = handleUnhandledException;
            execution.NotifyActivityFinished = notifyOfActivityFinished;
            execution.NotifyOfActivity = notifyOfActivity;
            

            // Operation should be in separate easily testable class
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
