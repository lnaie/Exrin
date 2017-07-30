namespace Exrin.Abstraction
{
    using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows.Input;

    public interface IRelayCommand: ICommand
    {
        int Timeout { get; set; }
        Action FinishedCallback { get; set; }
        bool Executing { get; }
        void OnCanExecuteChanged();
		Func<object, CancellationToken, Task<IList<IResult>>> Function { get; }

	}
}
