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
        public object ActiveStackIdentifier => throw new NotImplementedException();

        public IViewContainer ActiveViewContainer => throw new NotImplementedException();

        public Task<object> BuildView(string key, object args)
        {
            throw new NotImplementedException();
        }

        public Task GoBack()
        {
            throw new NotImplementedException();
        }

        public Task GoBack(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Init(object stackIdentifier, INavigationProxy page, bool showNavigationBar)
        {
            throw new NotImplementedException();
        }

        public void Init(Action<object> setRoot)
        {
            throw new NotImplementedException();
        }

        public void Init(Action<object> setRoot, Func<object> rootPage)
        {
            throw new NotImplementedException();
        }

        public Task LoadStack(Dictionary<string, object> definitions)
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

        public Task Navigate(string viewKey, object args, bool newInstance)
        {
            throw new NotImplementedException();
        }

        public Task Navigate(object stackIdentifier, string key, object args)
        {
            throw new NotImplementedException();
        }

        public Task Navigate(string key, object args, IStackOptions options)
        {
            throw new NotImplementedException();
        }

        public StackResult Navigate(IStackOptions options)
        {
            throw new NotImplementedException();
        }

        public Task Navigate(string viewKey, object args, bool newInstance, bool popSource)
        {
            throw new NotImplementedException();
        }

        public Task SilentPop(object stackIdentifier, IList<string> viewKeys)
        {
            throw new NotImplementedException();
        }

        Task INavigationService.Navigate<TViewModel>(object args)
        {
            throw new NotImplementedException();
        }

        void INavigationService.RegisterViewContainer<T>()
        {
            throw new NotImplementedException();
        }
    }
}
