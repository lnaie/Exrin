using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Exrin.Framework
{
    public abstract class ViewModel : BindableModel, IViewModel
    {
        protected IExecution Execution { get; set; }
        protected readonly IDisplayService _displayService = null;
        protected readonly INavigationService _navigationService = null;
        protected readonly IErrorHandlingService _errorHandlingService = null;
        protected readonly IStackRunner _stackRunner = null;

        public ViewModel(IDisplayService displayService, INavigationService navigationService,
            IErrorHandlingService errorHandlingService, IStackRunner stackRunner)

        {
            _displayService = displayService;
            _navigationService = navigationService;
            _errorHandlingService = errorHandlingService;
            _stackRunner = stackRunner;

            Execution = new Execution()
            {
                HandleTimeout = TimeoutHandle,
                NotifyOfActivity = NotifyActivity,
                NotifyActivityFinished = NotifyActivityFinished,
                HandleResult = HandleResult
            };

        }

        public abstract IVisualState VisualState { get; set; }

        private IDictionary<string, IRelayCommand> commands = new Dictionary<string, IRelayCommand>();
        public IRelayCommand GetCommand(Func<IRelayCommand> create, [CallerMemberName] string name = "")
        {
            if (!commands.ContainsKey(name))
                commands.Add(name, create());

            return commands[name];
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
                        case ResultType.PropertyUpdate:
                            var propertyArg = result.Arguments as PropertyArgs;
                            if (propertyArg == null)
                                break;

                            try
                            {
                                var propertyInfo = this.GetType().GetRuntimeProperty(propertyArg.Name);
                                propertyInfo.SetValue(this, propertyArg.Value);
                            }
                            catch (Exception ex)
                            {
                                await _errorHandlingService.ReportError(ex);
                                await _displayService.ShowDialog($"Unable to update property {propertyArg.Name}");
                            }

                            break;
                    }

                };
            }
        }
    }
}
