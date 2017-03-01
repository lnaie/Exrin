using Exrin.Abstraction;
using Exrin.Framework;
using ExrinSampleMobileApp.Framework.Abstraction.Model;
using ExrinSampleMobileApp.Logic.Base;
using ExrinSampleMobileApp.Logic.VisualState;

namespace ExrinSampleMobileApp.Logic.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthModel _model;

        public LoginViewModel(IAuthModel model, IExrinContainer exrinContainer) :
           base(exrinContainer, new LoginVisualState(model))
        {
            _model = model;
        }

        public IRelayCommand LoginCommand
        {
            get
            {
                return GetCommand(() =>
                {
                    return Execution.ViewModelExecute(new LoginOperation(_model));
                });
            }
        }

    }
}
