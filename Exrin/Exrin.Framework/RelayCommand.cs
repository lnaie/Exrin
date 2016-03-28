using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Exrin.Framework
{
    public class RelayCommand : ICommand, IRelayCommand
    {
        private readonly Action<object> _action = null;
        public RelayCommand(Action<object> action)
        {
            _action = action;
        }


        public bool Executing { get; private set; } = false;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Executing = true;

            _action(parameter);

            Executing = false;
        }
    }
}
