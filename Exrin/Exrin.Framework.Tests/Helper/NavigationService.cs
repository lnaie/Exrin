using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework.Tests.Helper
{
    public class NavigationService : INavigationService
    {



        public Task GoBack()
        {
            throw new NotImplementedException();
        }

        public Task GoBack(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Init(object stackIdentifier, INavigationContainer page, bool showNavigationBar)
        {
            throw new NotImplementedException();
        }

        public void Map(object stackIdentifier, string key, Type viewType, Type viewModelType)
        {
            throw new NotImplementedException();
        }

        public Task Navigate(string pageKey)
        {
            throw new NotImplementedException();
        }

        public Task Navigate(string pageKey, object args)
        {
            throw new NotImplementedException();
        }
    }
}
