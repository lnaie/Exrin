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

        void SetNavigationBar(bool isVisible, object page);

        bool CanGoBack();
            
        Task PushAsync(object page);

        Task PopAsync();

        Task PopAsync(object parameter);

        event EventHandler<IPageNavigationArgs> OnPopped;

        string CurrentPageKey { get; set; }

    }
}
