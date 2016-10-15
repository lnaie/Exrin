namespace Exrin.Abstraction
{
    using System.Collections.ObjectModel;

    public interface ITabbedView
    {
        ObservableCollection<object> Children { get; set; }        
    }
}
