namespace Exrin.Abstraction
{
    using System.Collections.ObjectModel;

    public interface ITabbedViewProxy
    {
        /// <summary>
        /// Native views of the children of the TabbedView
        /// </summary>
        ObservableCollection<object> Children { get; set; }        
    }
}
