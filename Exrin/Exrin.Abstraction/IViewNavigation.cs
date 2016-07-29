namespace Exrin.Abstraction
{
    public interface IViewNavigationArgs
    {
        IView PoppedView { get; set; }

        IView CurrentView { get; set; } 
    }
}
