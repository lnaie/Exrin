using System.Threading.Tasks;
using Exrin.Abstraction;
using Exrin.Framework;
using ExrinSampleMobileApp.Framework.Abstraction.Model;
using ExrinSampleMobileApp.Logic.Base;
using ExrinSampleMobileApp.Logic.VisualState;
using ExrinSampleMobileApp.Framework.Locator;
using ExrinSampleMobileApp.Framework.Locator.Views;

namespace ExrinSampleMobileApp.Logic.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private IMainModel _model;
        public MainViewModel(IMainModel model)
        {
            _model = model;
        }

        public override Task OnNavigated(object args)
        {
            return base.OnNavigated(args);
        }

        public override Task OnBackNavigated(object args)
        {
            return base.OnBackNavigated(args);
        }
		
        public IRelayCommand AboutCommand
        {
            get
            {
                return GetCommand(() =>
                {
                    return Execution.ViewModelExecute((parameter, token) =>
					{
						return new NavigationResult(Stacks.Main, Main.About);
					});
                });
            }
        }
    }
}
