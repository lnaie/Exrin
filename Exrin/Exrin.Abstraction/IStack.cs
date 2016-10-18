namespace Exrin.Abstraction
{
    using System.Threading.Tasks;

    public interface IStack
    {
        object StackIdentifier { get; }
        bool ShowNavigationBar { get; set; }
        StackStatus Status { get; }
        INavigationProxy Proxy { get; }
        Task StartNavigation(object args = null, bool loadStartKey = true);
        void Init();
        string NavigationStartKey { get; }
        Task Navigate(string key, object args);
        void StateChange(StackStatus state); // Notifies the stack its state is changing
        Task StackChanged();
        Task GoBack();
        Task GoBack(object parameter);

    }
}
