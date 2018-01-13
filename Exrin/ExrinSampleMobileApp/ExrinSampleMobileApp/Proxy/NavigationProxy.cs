using Exrin.Abstraction;
using Exrin.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExrinSampleMobileApp.Proxy
{
	public class NavigationProxy : INavigationProxy
	{

		private NavigationPage _page = null;
		public event EventHandler<IViewNavigationArgs> OnPopped;
		private Queue<object> _argQueue = new Queue<object>();
		public VisualStatus ViewStatus { get; set; } = VisualStatus.Unseen;

		public NavigationProxy()
		{
			_page = new NavigationPage();
			_page.Popped += _page_Popped;
		}

		public NavigationProxy(NavigationPage page)
		{
			_page = page;
			_page.Popped += _page_Popped;
		}

		private void _page_Popped(object sender, NavigationEventArgs e)
		{
			if (OnPopped != null)
			{
				var poppedPage = e.Page as IView;
				var currentPage = _page.CurrentPage as IView;
				var parameter = _argQueue.Count > 0 ? _argQueue.Dequeue() : null;
				OnPopped(this, new ViewNavigationArgs() { Parameter = parameter, CurrentView = currentPage, PoppedView = poppedPage });
			}
		}

		public void SetNavigationBar(bool isVisible, object page)
		{
			var bindableObject = page as BindableObject;
			if (bindableObject != null)
				NavigationPage.SetHasNavigationBar(bindableObject, isVisible);
		}

		public object NativeView { get { return _page; } }

		public bool CanGoBack()
		{
			return _page.Navigation.NavigationStack.Count > 1;
		}

		public async Task PopAsync(object parameter)
		{
			_argQueue.Enqueue(parameter);
			await _page.PopAsync();
		}

		public async Task PopAsync()
		{
			CloseMenu();
			await _page.PopAsync();			
		}

		public async Task PushAsync(object page)
		{
			var xamarinPage = page as Page;

			if (xamarinPage == null)
				throw new Exception("PushAsync can not push a non Xamarin Page");
			CloseMenu();
			await _page.PushAsync(xamarinPage, true);
		}

		private void CloseMenu()
		{
			if (Application.Current.MainPage is MasterDetailPage masterDetailPage)
				masterDetailPage.IsPresented = false;
		}

		public async Task ShowDialog(IDialogOptions dialogOptions)
		{
			if (ViewStatus == VisualStatus.Visible)
			{
				if (string.IsNullOrEmpty(dialogOptions.CancelButtonText))
				{
					await _page.DisplayAlert(dialogOptions.Title, dialogOptions.Message, string.IsNullOrEmpty(dialogOptions.OkButtonText) ? "OK" : dialogOptions.OkButtonText);
					dialogOptions.Result = true;
				}
				else
				{
					dialogOptions.Result = await _page.DisplayAlert(dialogOptions.Title, dialogOptions.Message, string.IsNullOrEmpty(dialogOptions.OkButtonText) ? "OK" : dialogOptions.OkButtonText, dialogOptions.CancelButtonText);
				}
			}
			else
			{
				throw new Exception("You can not call ShowDialog on a non-visible page");
			}
		}

		public Task ClearAsync()
		{
			_page = new NavigationPage();
			return Task.FromResult(true);
		}

		public Task SilentPopAsync(int indexFromTop)
		{
			var page = _page.Navigation.NavigationStack[_page.Navigation.NavigationStack.Count - indexFromTop - 1];
			_page.Navigation.RemovePage(page);
            // Because a remove page, doesn't issue a pop
            OnPopped?.Invoke(this, new ViewNavigationArgs() { Parameter = null, CurrentView = _page.CurrentPage as IView, PoppedView = page as IView });
            return Task.FromResult(true);
		}
	}
}
