using Exrin.Abstraction;
using Exrin.Framework;
using ExrinSampleMobileApp.Framework.Abstraction.Model;
using ExrinSampleMobileApp.Logic.Base;
using ExrinSampleMobileApp.Logic.VisualState;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExrinSampleMobileApp.Logic.ViewModel
{
	public class AboutViewModel : BaseViewModel
    {
		public AboutViewModel(IMainModel model) : base(new AboutVisualState(model)) {
			((AboutVisualState)VisualState).MyProperty = new System.Net.Http.HttpClient();
			Execution.PreCheck = (arg) => { return Task.FromResult(true); };
		}

        public IRelayCommand SettingsCommand
        {
            get
            {
                return GetCommand(() =>
                {
					
					return Execution.ViewModelExecute(new ViewModelExecute());
                });
            }
        }
    }

	public class ViewModelExecute : IViewModelExecute
	{
		public List<IBaseOperation> Operations => new List<IBaseOperation>()
		{
			new SettingsOperation(null)
		};

		public int TimeoutMilliseconds => 10000;
	}

}
