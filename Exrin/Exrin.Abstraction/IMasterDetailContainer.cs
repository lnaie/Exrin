namespace Exrin.Abstraction
{

    public interface IMasterDetailContainer: IViewContainer
    {
        IMasterDetailView MasterView { get; set; }
        IStack Master { get; set; }
        IStack Detail { get; set; }
    
    }
}
