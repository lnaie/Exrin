namespace Exrin.Framework
{
    using Abstraction;

    public class ViewNavigationArgs : IViewNavigationArgs
    {
        public IView CurrentView { get; set; }

        public IView PoppedView { get; set; }

        public object Parameter { get; set; }

        public PopType PopType { get; set; } = PopType.TopView;
    }
}
