using Exrin.Abstraction;
using Xamarin.Forms;

namespace ExrinSampleMobileApp.Proxy
{
	public class MasterDetailProxy : IMasterDetailProxy
	{
		private MasterDetailPage _mdp;
		
		public MasterDetailProxy(MasterDetailPage masterDetailPage)
		{
			View = _mdp = masterDetailPage;
		}

		public object DetailNativeView
		{
			get
			{
				return _mdp.Detail;
			}
			set
			{
				_mdp.Detail = value as Page;
			}
		}

		public object MasterNativeView
		{
			get
			{
				return _mdp.Master;
			}
			set
			{
				var page = value as Page;
				if (string.IsNullOrEmpty(page.Title))
					page.Title = "Please set your MasterPage Title";

				_mdp.Master = page as Page;
			}
		}

		public object View { get; set; }
	}
}
