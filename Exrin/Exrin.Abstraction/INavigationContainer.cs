using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface INavigationContainer
    {
        object View { get; }

        void SetNavigationBar(bool isVisible, object view); 

        bool CanGoBack();
            
        Task PushAsync(object view);

        Task PopAsync();

        Task PopAsync(object parameter);

        event EventHandler<IViewNavigationArgs> OnPopped;

        string CurrentViewKey { get; set; }

        Task ShowDialog(IDialogOptions dialogOptions);

    }
}
