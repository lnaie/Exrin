using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IViewModel
    {

        Task OnNavigated(object args);

        Task OnBackNavigated(object args);

        void OnAppearing();

        void OnDisappearing();

        void OnPopped();

    }
}
