namespace Exrin.Abstraction
{

    public interface IMasterDetailContainer : IViewContainer
    {
        IStack MasterStack { get; set; }
        IStack DetailStack { get; set; }
        IMasterDetailProxy Proxy { get; set; }	
		bool IsPresented { get; set; }
		void SetStack(ContainerType type, object page);
    }
}
