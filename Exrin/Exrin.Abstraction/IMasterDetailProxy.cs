namespace Exrin.Abstraction
{
    public interface IMasterDetailProxy
    {
        object MasterView { get; set; }
        object DetailView { get; set; }
    }
}
