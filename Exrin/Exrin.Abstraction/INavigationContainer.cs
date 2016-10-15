namespace Exrin.Abstraction
{
    using System;
    using System.Threading.Tasks;

    public interface INavigationContainer
    {
        object View { get; }

        object CurrentView { get; }

        void SetNavigationBar(bool isVisible, object view); 

        bool CanGoBack();
            
        Task PushAsync(object view);

        Task PopAsync();

        Task PopAsync(object parameter);

        Task ClearAsync();

        event EventHandler<IViewNavigationArgs> OnPopped;

        string CurrentViewKey { get; set; }

        /// <summary>
        /// Will remove from the stack without using a Pop
        /// </summary>
        /// <param name="indexFromTop">How many from the current page to Pop. e.g. top page is 0, next page back is -1</param>
        /// <returns></returns>
        Task SilentPopAsync(int indexFromTop);

        Task ShowDialog(IDialogOptions dialogOptions);

        VisualStatus ViewStatus { get; set; }

    }
}
