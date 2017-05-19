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

    public class SecondStack : BaseStack
    {

        public SecondStack(IViewService viewService)
            : base(new NavigationProxy(new NavigationPage()), viewService, Stacks.Second)
        {
            ShowNavigationBar = false;
        }

        protected override void Map()
        {
			base.NavigationMap<DetailView, DetailViewModel>(nameof(Second.Detail));
		}

        public override string NavigationStartKey
        {
            get
            {
                return nameof(Second.Detail);
            }
        }

    }
}
