using Exrin.Abstraction;
using Exrin.Framework;
using ExrinSampleMobileApp.Framework.Abstraction.ModelState;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ExrinSampleMobileApp.Logic.ModelState
{
    public class MainModelState : Exrin.Framework.ModelState, IMainModelState
    {

		public MainModelState()
		{
			ObservableList = new ObservableCollection<string>() { "-1", "0" };

			Task.Run(async () =>
			{
				for(int i = 0; i < 100; i++)
				{
					await Task.Delay(1000);
					ObservableList.Add(i.ToString());
				}
			});
		}

		public ObservableCollection<string> ObservableList { get { return Get<ObservableCollection<string>>(); } set { Set(value); } }
	}
}
