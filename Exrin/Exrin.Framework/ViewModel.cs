using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class ViewModel: BindableObject, IViewModel
    {
        protected IExecution Execution { get; set; }

        protected IDisplayService _displayService = null;
        protected INavigationService _navigationService = null;
        protected IErrorHandlingService _errorHandlingService = null;
        protected IStackRunner _stackRunner = null;

        public ViewModel(IDisplayService displayService, INavigationService navigationService,
            IErrorHandlingService errorHandlingService, IStackRunner stackRunner)
           
        {
            _displayService = displayService;
            _navigationService = navigationService;
            _errorHandlingService = errorHandlingService;
            _stackRunner = stackRunner;

        }

        private bool _isBusy = false;
        public bool IsBusy { get { return _isBusy; } set { _isBusy = value; OnPropertyChanged(); } }

        public virtual Task OnNavigated(object args)
        {
            return Task.FromResult(0);
        }

        public virtual Task OnBackNavigated(object args)
        {
            return Task.FromResult(0);
        }

        public virtual void OnAppearing() { }

        public virtual void OnDisappearing() { }

        public virtual void OnPopped() { }


        protected Func<Task> TimeoutHandle
        {
            get
            {
                return async () =>
                {
                    await _displayService.ShowDialog("Timeout Occurred");
                };
            }
        }

        protected Func<Task> NotifyActivity
        {
            get
            {
                return () =>
                {

                    IsBusy = true;

                    return Task.FromResult(0);

                };
            }
        }


        protected Func<Task> NotifyActivityFinished
        {
            get
            {
                return () =>
                {
                    IsBusy = false;

                    return Task.FromResult(0);
                };
            }
        }

        protected Func<IResult, Task> HandleResult
        {
            get
            {
                return async (result) =>
                {

                    if (result == null)
                        return;

                    switch (result.ResultAction)
                    {
                        case ResultType.Navigation:
                            {
                                var args = result.Arguments as NavigationArgs;

                                // Determine Stack Change
                                _stackRunner.Run(args.StackType);

                                // Determine Page Load
                                await _navigationService.Navigate(Convert.ToString(args.PageIndicator), args.Parameter);

                                break;
                            }
                        case ResultType.Error:
                            await _errorHandlingService.ReportError(result.Arguments as Exception);
                            break;

                        case ResultType.Display:
                            await _displayService.ShowDialog((result.Arguments as DisplayArgs).Message);
                            break;
                    }

                };
            }
        }
    }
}
