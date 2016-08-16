namespace Exrin.Abstraction
{
    public interface IMasterView
    {
        object View { get; }
        object MasterView { get; set; }
        object DetailView { get; set; }
    }
}
