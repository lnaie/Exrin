namespace Exrin.Abstraction
{
    using System.Collections.Generic;
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
        Task Navigate(string key, object args, bool newInstance, bool popSource);
        void StateChange(StackStatus state); // Notifies the stack its state is changing
        Task StackChanged();
        Task GoBack();
        Task GoBack(object parameter);
        Task Navigate<TViewModel>(object args) where TViewModel : class, IViewModel;
        Task SilentPop(IList<string> viewKeys);

    }
}
