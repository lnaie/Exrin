using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Perf
{
    public class NavigationContainer : INavigationContainer
    {
        public object CurrentView { get; private set; }

        public string CurrentViewKey
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public object View
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public VisualStatus ViewStatus
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler<IViewNavigationArgs> OnPopped;

        public bool CanGoBack()
        {
            throw new NotImplementedException();
        }

        public Task ClearAsync()
        {
            throw new NotImplementedException();
        }

        public Task PopAsync()
        {
            throw new NotImplementedException();
        }

        public Task PopAsync(object parameter)
        {
            throw new NotImplementedException();
        }

        public Task PushAsync(object view)
        {
            throw new NotImplementedException();
        }

        public void SetNavigationBar(bool isVisible, object view)
        {
            throw new NotImplementedException();
        }

        public Task ShowDialog(IDialogOptions dialogOptions)
        {
            throw new NotImplementedException();
        }
    }
}
