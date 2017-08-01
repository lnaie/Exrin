namespace Exrin.Abstraction
{
	public interface IDisplayArgs: IResultArgs
    {
        string Title { get; set; }
        string Message { get; set; }
		string OKButtonText { get; set; }
    }
}
