namespace Exrin.Abstraction
{
    public interface INavigationArgs: IResultArgs
    {
        object StackType { get; set; }

        object Key { get; set; }

        object Parameter { get; set; }

        bool NewInstance { get; set; }

        /// <summary>
        /// Will silently pop the page it is coming from, once the page it is navigating to is shown.
        /// </summary>
        bool PopSource { get; set; }
    }
}
