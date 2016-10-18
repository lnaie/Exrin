namespace Exrin.Abstraction
{

    public interface IMasterDetailContainer : IViewContainer
    {
        IStack MasterStack { get; set; }
        IStack DetailStack { get; set; }
        IMasterDetailProxy Proxy { get; set; }

    }
}
