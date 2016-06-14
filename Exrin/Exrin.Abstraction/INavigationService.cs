using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface INavigationService
    {

        void Init(object stackIdentifier, INavigationContainer container, bool showNavigationBar);

        Task Navigate(string key);

        Task Navigate(string key, object args);

        Task GoBack();

        Task GoBack(object parameter);

        void Map(object stackIdentifier, string key, Type viewType, Type viewModelType);

    }
}
