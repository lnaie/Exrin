using Exrin.Abstraction;
using Exrin.Framework;
using System.Collections.ObjectModel;

namespace ExrinSampleMobileApp.Logic.VisualState
{
    public class MainVisualState : Exrin.Framework.VisualState
    {
        public MainVisualState(IBaseModel model) : base(model)
        {
        }

		
		public ObservableCollection<string> ObservableList { get { return Get<ObservableCollection<string>>(); } set { Set(value); } }
	}
}
