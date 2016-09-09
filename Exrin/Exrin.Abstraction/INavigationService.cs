namespace Exrin.Abstraction
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INavigationService
    {

        void Init(object stackIdentifier, INavigationContainer container, bool showNavigationBar);

        Task Navigate(string key);

        Task Navigate(string key, object args);

        Task LoadStack(Dictionary<string, object> definitions);

        Task GoBack();

        Task GoBack(object parameter);

        void Map(object stackIdentifier, string key, Type viewType, Type viewModelType);

        Task<object> BuildView(string key, object args);

    }
}
