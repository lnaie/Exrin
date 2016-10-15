namespace Exrin.Framework
{
    using Abstraction;

    public class ViewContainer : IViewContainer
    {
        public ViewContainer(string identifier, object view)
        {
            Identifier = identifier;
            View = view;
        }
        public string Identifier { get; private set; }

        public object View { get; set; }
    }
}
