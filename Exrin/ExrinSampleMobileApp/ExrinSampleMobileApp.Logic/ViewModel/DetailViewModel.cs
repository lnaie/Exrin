using Exrin.Abstraction;
using Exrin.Framework;
using ExrinSampleMobileApp.Framework.Abstraction.Model;
using ExrinSampleMobileApp.Logic.Base;
using ExrinSampleMobileApp.Logic.VisualState;

namespace ExrinSampleMobileApp.Logic.ViewModel
{
	public class DetailViewModel : BaseViewModel
    {
        private readonly IAuthModel _model;

        public DetailViewModel(IAuthModel model, IExrinContainer exrinContainer) :
           base(exrinContainer, new DetailVisualState(model))
        {
            _model = model;
        }

		public IRelayCommand GoToAboutCommand
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
