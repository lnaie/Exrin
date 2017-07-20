namespace Exrin.Abstraction
{
    using System.Threading.Tasks;

    public interface IViewModel
    {
        Task OnPreNavigate(object args, Args e);

        Task OnNavigated(object args);

        Task OnBackNavigated(object args);

        void OnAppearing();

        void OnDisappearing();

        bool OnBackButtonPressed();

        void OnPopped();

        IVisualState VisualState { get; set; }

		/// <summary>
		/// Delay before setting IsBusy in milliseconds.
		/// </summary>
		int IsBusyDelay { get; set; }

    }
}
