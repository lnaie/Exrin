namespace Exrin.Abstraction
{
    public interface IDialogOptions
    {
        string Title { get; set; }
        string Message { get; set; }
        bool Result { get; set; }
        string OkButtonText { get; set; }
        string CancelButtonText { get; set; }
    }
}
