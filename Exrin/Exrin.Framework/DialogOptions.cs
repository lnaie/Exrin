namespace Exrin.Framework
{
    using Abstraction;

    public class DialogOptions : IDialogOptions
    {
        public string Message { get; set; }

		public bool Result { get; set; }

		public string Title { get; set; }

        public string OkButtonText { get; set; }

        public string CancelButtonText { get; set; }
    }
}
