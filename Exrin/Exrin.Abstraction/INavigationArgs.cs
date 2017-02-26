namespace Exrin.Abstraction
{
    public interface INavigationArgs: IResultArgs
    {
        object StackType { get; set; }

        object Key { get; set; }

        object Parameter { get; set; }

        bool NewInstance { get; set; }
    }
}
