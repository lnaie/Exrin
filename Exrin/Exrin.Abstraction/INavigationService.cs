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

        Task GoBack();

        Task GoBack(object parameter);

        void Map(object stackIdentifier, string key, Type viewType, Type viewModelType, bool noHistory = false);

        /// <summary>
        /// WARNING: I shouldn't be exposing this. Please don't base anything off this it will be refactored later
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<object> BuildView(string key, object args);

        Task StackChanged();

    }
}
