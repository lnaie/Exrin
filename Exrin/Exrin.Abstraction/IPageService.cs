using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IPageService
    {

        void Map(Type pageType, Type viewModelType);

        Task<object> Build(Type pageType, object parameter);

        object GetBindingContext(Type pageType);

    }
}
