using Exrin.Abstraction;
using Exrin.Framework;
using ExrinSampleMobileApp.Framework.Abstraction.Model;
using ExrinSampleMobileApp.Logic.Base;
using ExrinSampleMobileApp.Logic.VisualState;

namespace ExrinSampleMobileApp.Logic.ViewModel
{
    public class AboutViewModel : BaseViewModel
    {
		public AboutViewModel(IExrinContainer exrinContainer, IMainModel model) : base(exrinContainer, new AboutVisualState(model)) { }

        public IRelayCommand SettingsCommand
        {
            get
            {
                return GetCommand(() =>
                {
                    return Execution.ViewModelExecute(new SettingsOperation());
                });
            }
        }
    }
}
