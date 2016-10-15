using System.Collections.ObjectModel;

namespace Exrin.Abstraction
{
    public interface ITabbedView
    {
        object View { get; }
        ObservableCollection<INavigationView> Children { get; set; }        
    }
}
