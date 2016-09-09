namespace Exrin.Abstraction
{
    public interface IMasterDetailView
    {
        object View { get; }
        object MasterView { get; set; }
        object DetailView { get; set; }
    }
}
