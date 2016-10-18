namespace Exrin.Abstraction
{
    using System.Collections.ObjectModel;

    public interface ITabbedViewProxy
    {
        ObservableCollection<object> Children { get; set; }        
    }
}
