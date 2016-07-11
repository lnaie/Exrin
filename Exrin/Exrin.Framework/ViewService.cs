using Exrin.Abstraction;
using Exrin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class ViewService : IViewService
    {
        private readonly IInjection _injection = null;

        public ViewService(IInjection injection)
        {
            _injection = injection;
        }

        private readonly IDictionary<Type, Type> _viewsByType = new Dictionary<Type, Type>();

        private object GetBindingContext(Type viewType)
        {
            var viewModelType = _viewsByType[viewType];

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

        public async Task<object> Build(Type viewType, object parameter)
        {
            ConstructorInfo constructor = null;
            object[] parameters = null;

            constructor = viewType.GetTypeInfo()
                .DeclaredConstructors
                .FirstOrDefault(c => !c.GetParameters().Any());

            parameters = new object[] { };

            if (constructor == null)
                throw new InvalidOperationException(
                    $"No suitable constructor found for view {viewType.ToString()}");

            IView view = null;
            await ThreadHelper.RunOnUIThreadAsync(() =>
            {
                view = constructor.Invoke(parameters) as IView;
                return Task.FromResult(true);
            });

            if (view == null)
                throw new InvalidOperationException(
                    $"View {viewType.ToString()} does not implement the interface {nameof(IView)}");

            // Assign Binding Context
            if (_viewsByType.ContainsKey(viewType))
            {
                view.BindingContext = GetBindingContext(viewType);

                // Pass parameter to view model if applicable
                var model = view.BindingContext as IViewModel;
                if (model != null)
                    await model.OnNavigated(parameter);

                var multiView = view as IMultiView;

                if (multiView != null)
                    foreach (var p in multiView.Views)
                        p.BindingContext = GetBindingContext(p.GetType());

            }
            else
                throw new InvalidOperationException(
                    "No suitable view model found for view " + viewType.ToString());

            return view;
        }

        public void Map(Type viewType, Type viewModelType)
        {
            lock (_viewsByType)
                if (_viewsByType.ContainsKey(viewType))
                    _viewsByType[viewType] = viewModelType;
                else
                    _viewsByType.Add(viewType, viewModelType);
        }
    }
}
