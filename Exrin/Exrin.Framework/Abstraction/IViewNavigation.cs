namespace Exrin.Abstraction
{
    public interface IViewNavigationArgs
    {
        IView PoppedView { get; set; }

        IView CurrentView { get; set; } 

        object Parameter { get; set; }

        /// <summary>
        /// Default is TopView, which means the top most view is being popped
        /// </summary>
        PopType PopType { get; set; }
    }
}
