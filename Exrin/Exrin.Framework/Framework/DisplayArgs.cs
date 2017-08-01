namespace Exrin.Framework
{
	using Abstraction;

	public class DisplayArgs: IDisplayArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }
		public string OKButtonText { get; set; } = "OK";
	}
}
