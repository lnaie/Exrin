using Exrin.Abstraction;
using Exrin.Framework;
using ExrinSampleMobileApp.Logic.ViewModel;
using ExrinSampleMobileApp.Proxy;
using ExrinSampleMobileApp.View;
using Xamarin.Forms;

namespace ExrinSampleMobileApp.Logic.Stack
{
    using Framework.Locator;
    using Framework.Locator.Views;

    public class MainStack : BaseStack
    {

        public MainStack(IViewService viewService)
            : base(new NavigationProxy(new NavigationPage()), viewService, Stacks.Main)
        {
            ShowNavigationBar = false;
        }

        protected override void Map()
        {
            base.NavigationMap<MainView, MainViewModel>(nameof(Main.Main));
            base.NavigationMap<AboutView, AboutViewModel>(nameof(Main.About));
            base.NavigationMap<SettingsView, SettingsViewModel>(nameof(Main.Settings));
			base.NavigationMap<View.ListView, ListViewModel>(nameof(Main.List));
			base.NavigationMap<DetailView, DetailViewModel>(nameof(Main.Detail));
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
