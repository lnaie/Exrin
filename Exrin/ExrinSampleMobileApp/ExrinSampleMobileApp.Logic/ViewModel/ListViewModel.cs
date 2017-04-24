using Exrin.Abstraction;
using Exrin.Framework;
using ExrinSampleMobileApp.Framework.Abstraction.Model;
using ExrinSampleMobileApp.Logic.Base;
using ExrinSampleMobileApp.Logic.VisualState;

namespace ExrinSampleMobileApp.Logic.ViewModel
{
    public class ListViewModel : BaseViewModel
    {
        private readonly IAuthModel _model;

        public ListViewModel(IAuthModel model, IExrinContainer exrinContainer) :
           base(exrinContainer, new ListVisualState(model))
        {
            _model = model;
        }

		public IRelayCommand GoToDetailCommand
		{
			get
			{
				return GetCommand(() =>
				{
					return Execution.ViewModelExecute(new DetailOperation());
				});
			}
		}

	}
}
