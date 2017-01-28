namespace Exrin.Abstraction
{
    using System;
    using System.Windows.Input;

    public interface IRelayCommand: ICommand
    {
        int Timeout { get; set; }
        Action FinishedCallback { get; set; }
        bool Executing { get; }
        void OnCanExecuteChanged();
    }
}
