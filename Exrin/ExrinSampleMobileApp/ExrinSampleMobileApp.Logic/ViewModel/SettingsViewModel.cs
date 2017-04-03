using Exrin.Abstraction;
using Exrin.Framework;
using ExrinSampleMobileApp.Framework.Abstraction.Model;
using ExrinSampleMobileApp.Logic.Base;
using ExrinSampleMobileApp.Logic.VisualState;

namespace ExrinSampleMobileApp.Logic.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel(IExrinContainer exrinContainer, IMainModel model) : base(exrinContainer, new SettingsVisualState(model))
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
