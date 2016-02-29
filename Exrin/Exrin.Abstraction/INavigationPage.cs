using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface INavigationPage
    {

        object Page { get; }

        bool CanGoBack();

        Task PushAsync(object page);

        Task PopAsync();


    }
}
