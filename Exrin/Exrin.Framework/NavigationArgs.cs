namespace Exrin.Framework
{
    using Abstraction;

    public class NavigationArgs: INavigationArgs
    {

        public object StackType { get; set; }

        public object Key { get; set; }

        public object Parameter { get; set; }

    }
}
