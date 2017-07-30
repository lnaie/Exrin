namespace Exrin.Abstraction
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INavigationService
    {
        /// <summary>
        /// Provides the action to set the main root of the visual hierarchy. e.g. (view) => { MainPage = view as Page }
        /// </summary>
        /// <param name="setRoot"></param>
        [Obsolete("Please use void Init(Action<object> setRoot, object rootPage);")]
        void Init(Action<object> setRoot);
        
        void Init(Action<object> setRoot, Func<object> rootPage);

        Task Navigate(string key);

        Task Navigate(string key, object args);

        Task Navigate(string viewKey, object args, bool newInstance);

        Task Navigate(string viewKey, object args, bool newInstance, bool popSource);

        Task Navigate<TViewModel>(object args) where TViewModel : class, IViewModel;

        Task Navigate(object stackIdentifier, string key, object args);

        Task Navigate(string key, object args, IStackOptions options);

        StackResult Navigate(IStackOptions options);

		Task Navigate(object containerId, object regionId, object stackIdentifier, string key, object args);

		Task Navigate(object containerId, object regionId, string key, object args, IStackOptions options);

		StackResult Navigate(object containerId, object regionId, IStackOptions options);

		Task GoBack();

        Task GoBack(object parameter);

        void RegisterViewContainer<T>() where T : class, IViewContainer;
      
        Task SilentPop(object stackIdentifier, IList<string> viewKeys);

		void RegisterStack(IStack stack);

    }
}
