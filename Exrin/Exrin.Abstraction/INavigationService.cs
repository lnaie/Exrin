namespace Exrin.Abstraction
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INavigationService
    {

        void Init(Action<object> setRoot);

        Task Navigate(string key);

        Task Navigate(string key, object args);

        Task Navigate(object stackIdentifier, string viewKey, object args);

        Task Navigate(string viewKey, object args, IStackOptions options);

        Task GoBack();

        Task GoBack(object parameter);

        void RegisterViewContainer<T>() where T : class, IViewContainer;

        StackResult Navigate(IStackOptions options);

        void Rebuild();

        object CurrentStack { get; } //TODO: Check to remove

    }
}
