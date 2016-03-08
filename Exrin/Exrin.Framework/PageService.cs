using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class PageService : IPageService
    {
        private readonly IInjection _injection = null;

        public PageService(IInjection injection)
        {
            _injection = injection;
        }

        private readonly IDictionary<Type, Type> _pagesByType = new Dictionary<Type, Type>();

        private object GetBindingContext(Type pageType)
        {
            var viewModelType = _pagesByType[pageType];

            ConstructorInfo constructor = null;

            var parameters = new object[] { };

            constructor = viewModelType.GetTypeInfo()
                   .DeclaredConstructors
                   .FirstOrDefault(c => !c.GetParameters().Any());

            if (constructor == null)
            {
                constructor = viewModelType.GetTypeInfo()
                   .DeclaredConstructors.First();

                var parameterList = new List<object>();

                foreach (var param in constructor.GetParameters())
                    parameterList.Add(_injection.Get(param.ParameterType));

                parameters = parameterList.ToArray();
            }

            if (constructor == null)
                throw new InvalidOperationException(
                    $"No suitable constructor found for ViewModel {viewModelType.ToString()}");
            
            return constructor.Invoke(parameters);
        }

        public async Task<object> Build(Type pageType, object parameter)
        {
            ConstructorInfo constructor = null;
            object[] parameters = null;

            if (parameter == null)
            {
                constructor = pageType.GetTypeInfo()
                    .DeclaredConstructors
                    .FirstOrDefault(c => !c.GetParameters().Any());

                parameters = new object[] { };
            }
            else
            {
                constructor = pageType.GetTypeInfo()
                    .DeclaredConstructors
                    .FirstOrDefault(
                        c =>
                        {
                            var p = c.GetParameters();
                            return p.Count() == 1
                                   && p[0].ParameterType == parameter.GetType();
                        });

                parameters = new[]
                            {
                                        parameter
                                    };
            }

            if (constructor == null)
                throw new InvalidOperationException(
                    $"No suitable constructor found for page {pageType.ToString()}");

            var page = constructor.Invoke(parameters) as IPage;

            if (page == null)
                throw new InvalidOperationException(
                    $"Page {pageType.ToString()} does not implement the interface IPage");

            // Assign Binding Context
            if (_pagesByType.ContainsKey(pageType))
            {
                page.BindingContext = GetBindingContext(pageType);

                // Pass parameter to view model if applicable
                var model = page.BindingContext as IViewModel;
                if (model != null)
                    await model.OnNavigated(parameter);
            }
            else
                throw new InvalidOperationException(
                    "No suitable view model found for page " + pageType.ToString());

            return page;
        }

        public void Map(Type pageType, Type viewModelType)
        {
            lock (_pagesByType)
            {
                if (_pagesByType.ContainsKey(pageType))
                {
                    _pagesByType[pageType] = viewModelType;
                }
                else
                {
                    _pagesByType.Add(pageType, viewModelType);
                }
            }
        }
    }
}
