namespace Exrin.Framework
{
	using Abstraction;
	using System;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using System.Collections.Generic;
	using System.Threading;
	using System.Diagnostics;

	public class RelayCommand<T> : RelayCommand
	{
		public RelayCommand(Func<T, Task> action) : base(new Action<object>(o => action((T)o))) { }
		public RelayCommand(Func<T, Task> action, Func<object, bool> canExecute) : base(new Action<object>(o => action((T)o)), canExecute) { }
		public RelayCommand(Action<T> action) : base(new Action<object>(o => action((T)o))) { }
		public RelayCommand(Action<T> action, Func<object, bool> canExecute) : base(new Action<object>(o => action((T)o)), canExecute) { }

	}


	public class RelayCommand : ICommand, IRelayCommand
	{
		private readonly Func<object, bool> _canExecute = null;
		private readonly Func<object, Task> _action = null;
		public RelayCommand(Func<object, Task> action)
		{
			_action = action;
		}
		public RelayCommand(Func<object, Task> action, Func<object, bool> canExecute)
		{
			_action = action;
			_canExecute = canExecute;
		}
		public RelayCommand(Action<object> action)
		{
			_action = (parameter) => { action(parameter); return Task.FromResult(true); };
		}
		public RelayCommand(Action<object> action, Func<object, bool> canExecute)
		{
			_action = (parameter) => { action(parameter); return Task.FromResult(true); };
			_canExecute = canExecute;
		}

		public void OnCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, new EventArgs());
		}

		public bool Executing { get; private set; } = false;
		public Action FinishedCallback { get; set; } = null;
		public int Timeout { get; set; }

		public Func<object, CancellationToken, Task<IList<IResult>>> Function { get; internal set; }

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			if (_canExecute == null)
				return true;

			return _canExecute.Invoke(parameter);
		}

		public void Execute(object parameter)
		{

			if (!CanExecute(parameter))
				return;

			Executing = true;

			_action(parameter).ContinueWith((task) =>
			{
				Executing = false;

				if (task.IsFaulted)
					Debug.WriteLine($"{task.Exception.Message} {task.Exception.InnerException.StackTrace}");

				FinishedCallback?.Invoke();
			});
		}
	}
}
