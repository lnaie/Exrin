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

    public class AuthenticationStack : BaseStack
    {
        public AuthenticationStack(IViewService viewService)
            : base(new NavigationProxy(), viewService, Stacks.Authentication, nameof(Authentication.Login))
        {
            ShowNavigationBar = false;
        }
        protected override void Map()
        {
            base.NavigationMap<LoginView, LoginViewModel>(nameof(Authentication.Login));
        }

    }
}
