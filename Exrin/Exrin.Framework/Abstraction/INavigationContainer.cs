namespace Exrin.Abstraction
{
    public interface ISingleContainer: IViewContainer
    {
        IStack Stack { get; set; }
    }
}
