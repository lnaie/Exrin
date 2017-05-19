using Exrin.Abstraction;
using Exrin.Framework;
using ExrinSampleMobileApp.Framework.Abstraction.Model;
using ExrinSampleMobileApp.Logic.Base;
using ExrinSampleMobileApp.Logic.VisualState;

namespace ExrinSampleMobileApp.Logic.ViewModel
{
	public class MenuViewModel : BaseViewModel
    {
		public MenuViewModel(IExrinContainer exrinContainer, IMainModel model) : base(exrinContainer, new MenuVisualState(model)) {
			
		}

        public IRelayCommand SettingsCommand
        {
            get
            {
                return GetCommand(() =>
                {					
					return Execution.ViewModelExecute(new MenuOperation());
                });
            }
        }
    }
}
