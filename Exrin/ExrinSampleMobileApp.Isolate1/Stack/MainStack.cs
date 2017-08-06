using Exrin.Abstraction;
using Exrin.Framework;
using ExrinSampleMobileApp.Framework.Locator;
using ExrinSampleMobileApp.Framework.Locator.Views;
using ExrinSampleMobileApp.Isolate1.View;
using ExrinSampleMobileApp.Isolate1.ViewModel;

namespace ExrinSampleMobileApp.Isolate1.Stack
{
	public class MainStack : BaseStack
	{

		public MainStack(IViewService viewService, INavigationProxy proxy)
			: base(proxy, viewService, Stacks.MainTwo)
		{
			ShowNavigationBar = false;
		}

		protected override void Map()
		{
			base.NavigationMap<MainView, MainViewModel>(nameof(MainTwo.Main));
		}

		public override string NavigationStartKey
		{
			get
			{
				return nameof(MainTwo.Main);
			}
		}

	}
}
