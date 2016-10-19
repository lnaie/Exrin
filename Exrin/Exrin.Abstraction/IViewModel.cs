using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IViewModel
    {
        Task OnPreNavigate(object args);

        Task OnNavigated(object args);

        Task OnBackNavigated(object args);

        void OnAppearing();

        void OnDisappearing();

        bool OnBackButtonPressed();

        void OnPopped();

        IVisualState VisualState { get; set; }

    }
}
