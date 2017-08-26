using Exrin.Abstraction;
using Exrin.Framework;
using ExrinSampleMobileApp.Framework.Abstraction.Model;
using ExrinSampleMobileApp.Framework.Locator;
using ExrinSampleMobileApp.Framework.Locator.Views;
using ExrinSampleMobileApp.Logic.Base;
using System;
using System.Threading.Tasks;

namespace ExrinSampleMobileApp.Logic.ViewModel
{
	public class MainViewModel : BaseViewModel
    {
        private IMainModel _model;
        public MainViewModel(IMainModel model)
        {
            _model = model;

			IsBusyDelay = 1000; // In milliseconds
			
			Execution.PreCheck = async (arg) =>
			{
				//var flags = (PreChecks)arg;
				//if (flags.HasFlag(PreChecks.Authentication))
				//await Task.Delay(1000);

				return true;
			};
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
						// Anything I want in here. Example of NavigationResult shown.
						return new NavigationResult(Stacks.Main, Main.About);
					}, precheck: PreChecks.Authentication | PreChecks.InternetConnectivity);
                });
            }
        }
		[Flags]
		public enum PreChecks
		{
			Authentication,
			InternetConnectivity
		}

	}

}
