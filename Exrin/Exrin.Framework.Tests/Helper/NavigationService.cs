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

        public void Init(INavigationPage page)
        {
            throw new NotImplementedException();
        }

        public void Map(string pageKey, Type pageType)
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
