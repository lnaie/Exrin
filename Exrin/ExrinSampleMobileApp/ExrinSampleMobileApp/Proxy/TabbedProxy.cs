using Exrin.Abstraction;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace ExrinSampleMobileApp.Proxy
{
	public class TabbedProxy : ITabbedViewProxy
	{
		public event System.EventHandler PageChanged;
		public TabbedProxy(TabbedPage page)
		{

			View = page;
			Children.CollectionChanged += Children_CollectionChanged;

			page.CurrentPageChanged += Page_CurrentPageChanged;
		}

		public object CurrentPage
		{
			get
			{
				return ((TabbedPage)View).CurrentPage;
			}
			set
			{
				var tabbedPage = View as TabbedPage;
				tabbedPage.CurrentPage = value as Page;
			}
		}

		private void Page_CurrentPageChanged(object sender, System.EventArgs e)
		{
			PageChanged?.Invoke(sender, e);
		}

		private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			var page = View as TabbedPage;
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (var item in e.NewItems)
						page.Children.Add(item as Page);
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (var item in e.OldItems)
						page.Children.Remove(item as Page);
					break;
			}
		}

		public ObservableCollection<object> Children { get; set; } = new ObservableCollection<object>();

		public object View { get; set; }
	}
}
