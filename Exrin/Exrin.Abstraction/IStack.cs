namespace Exrin.Abstraction
{
    using System.Threading.Tasks;

    public interface IStack
    {
        object StackIdentifier { get; set; }
        bool ShowNavigationBar { get; set; }
        StackStatus Status { get; set; }
        INavigationContainer Container { get; }
        IMasterDetailView MasterView { get; }
        Task StartNavigation(object args = null, bool loadStartKey = true);
        void Init();
        string NavigationStartKey { get; }

    }
}
