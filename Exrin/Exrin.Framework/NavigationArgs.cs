namespace Exrin.Framework
{
    using Abstraction;

    public class NavigationArgs: INavigationArgs
    {
		public object ContainerId { get; set; }

		public object StackType { get; set; }

        public object Key { get; set; }

        public object Parameter { get; set; }

        public bool NewInstance { get; set; }

        public bool PopSource { get; set; }
    }
}
