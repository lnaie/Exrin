namespace Exrin.Abstraction
{

    public interface IViewContainer
    {
        string Identifier { get; }
        object NativeView { get; }
    }
}
