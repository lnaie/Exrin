using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IViewService
    {
        void Map(Type viewType, Type viewModelType);

        Task<object> Build(Type viewType, object parameter); 
    }
}
