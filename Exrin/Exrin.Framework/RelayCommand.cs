using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Exrin.Framework
{
    // TODO: This needs finishing - relays to ViewModelExecute or ModelExecute
    public class RelayCommand : ICommand, IRelayCommand
    {
        private readonly Action<object> _action = null;
        public RelayCommand(Action<object> action)
        {
            _action = action;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
