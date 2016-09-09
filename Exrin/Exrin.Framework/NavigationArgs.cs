namespace Exrin.Framework
{
    using Abstraction;

    public class NavigationArgs: INavigationArgs
    {

        public object StackType { get; set; }

        public object Key { get; set; }

        public object Parameter { get; set; }

        /// <summary>
        /// If this page already exists on the stack do you want to Pop pages until you
        /// reach it (true) or push another instance of the page (false)? Default (true)
        /// </summary>
        public bool BackTrack { get; set; } = true;

    }
}
