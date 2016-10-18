namespace Exrin.Abstraction
{

    public interface IMasterDetailContainer : IViewContainer
    {
        IStack Master { get; set; }
        IStack Detail { get; set; }
        IMasterDetailProxy Proxy { get; set; }

    }
}
