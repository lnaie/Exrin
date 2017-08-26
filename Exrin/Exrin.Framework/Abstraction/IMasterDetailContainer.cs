namespace Exrin.Abstraction
{

    public interface IMasterDetailContainer : IViewContainer
    {
        IHolder MasterStack { get; set; }
        IHolder DetailStack { get; set; }
        IMasterDetailProxy Proxy { get; set; }	
		bool IsPresented { get; set; }
		void SetStack(ContainerType type, object page);
    }
}
