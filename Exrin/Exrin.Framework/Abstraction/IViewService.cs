namespace Exrin.Abstraction
{
    using System;
    using System.Threading.Tasks;

    public interface IViewService
    {
        void Map(Type viewType, Type viewModelType);

        Type GetMap(Type viewModelType);

        Task<IView> Build(ITypeDefinition viewType); 
    }
}
