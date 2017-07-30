namespace Exrin.Abstraction
{
    public interface IMasterDetailProxy
    {
        object MasterNativeView { get; set; }
        object DetailNativeView { get; set; }
    }
}
