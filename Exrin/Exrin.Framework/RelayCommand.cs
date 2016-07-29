namespace Exrin.Framework
{
    using Abstraction;
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class RelayCommand : ICommand, IRelayCommand
    {
        private readonly Func<object, Task> _action = null;
        public RelayCommand(Func<object, Task> action)
        {
            _action = action;
        }

        public RelayCommand(Action<object> action)
        {
            _action = (parameter) => { action(parameter);  return Task.FromResult(true); };
        }
        
        public bool Executing { get; private set; } = false;
        public Action FinishedCallback { get; set; } = null;

        public int Timeout { get; set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Executing = true;

            _action(parameter).ContinueWith((task) =>
            {
                Executing = false;
                FinishedCallback?.Invoke();               
            });        
        }
    }
}
