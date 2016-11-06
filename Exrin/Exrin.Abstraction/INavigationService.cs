namespace Exrin.Abstraction
{
    using System;
    using System.Threading.Tasks;

    public interface INavigationService
    {
        /// <summary>
        /// Provides the action to set the main root of the visual hierarchy. e.g. (view) => { MainPage = view as Page }
        /// </summary>
        /// <param name="setRoot"></param>
        void Init(Action<object> setRoot);

        Task Navigate(string key);

        Task Navigate(string key, object args);

        Task Navigate<TViewModel>(object args) where TViewModel : class, IViewModel;

        Task Navigate(object stackIdentifier, string key, object args);

        Task Navigate(string key, object args, IStackOptions options);

        StackResult Navigate(IStackOptions options);

        Task GoBack();

        Task GoBack(object parameter);

        void RegisterViewContainer<T>() where T : class, IViewContainer;
      
        object ActiveStackIdentifier { get; }

    }
}
