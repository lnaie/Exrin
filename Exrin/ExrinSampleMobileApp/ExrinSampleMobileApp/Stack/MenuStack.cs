using Exrin.Abstraction;
using Exrin.Framework;
using ExrinSampleMobileApp.Logic.ViewModel;
using ExrinSampleMobileApp.Proxy;
using ExrinSampleMobileApp.View;
using Xamarin.Forms;

namespace ExrinSampleMobileApp.Logic.Stack
{
	using ExrinSampleMobileApp.Framework.Locator;
	using Framework.Locator.Views;

	public class MenuStack : BaseStack
    {

        public MenuStack(IViewService viewService)
            : base(new NavigationProxy(new NavigationPage()), viewService, Stacks.Menu)
        {
            ShowNavigationBar = false;
        }

        protected override void Map()
        {
			base.NavigationMap<MenuView, MenuViewModel>(nameof(Menu.Main));
           
		}

        public override string NavigationStartKey
        {
            get
            {
                return nameof(Main.Main);
            }
        }

    }
}
