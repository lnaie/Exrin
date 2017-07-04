using Exrin.Abstraction;
using Exrin.Framework;
using ExrinSampleMobileApp.Framework.Abstraction.Model;
using ExrinSampleMobileApp.Logic.Base;
using ExrinSampleMobileApp.Logic.VisualState;

namespace ExrinSampleMobileApp.Logic.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel(IMainModel model) : base(new SettingsVisualState(model))
        {
        }

        public IRelayCommand BackToMainCommand
        {
            get
            {
                return GetCommand(() =>
                {
                    return Execution.ViewModelExecute(new BackToMainOperation());
                });
            }
        }
    }
}
