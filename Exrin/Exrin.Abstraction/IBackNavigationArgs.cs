namespace Exrin.Abstraction
{
    public interface IBackNavigationArgs: IResultArgs
    {
        // TODO: Might want to expand to say how many to go back?

        object Parameter { get; set; }
    }
}
