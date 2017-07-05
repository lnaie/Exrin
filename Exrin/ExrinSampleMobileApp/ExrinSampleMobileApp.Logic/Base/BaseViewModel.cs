using Exrin.Abstraction;

namespace ExrinSampleMobileApp.Logic.Base
{
    public class BaseViewModel : Exrin.Framework.ViewModel
    {
		public BaseViewModel() : base() { }
        public BaseViewModel(IVisualState visualState, string caller = nameof(BaseViewModel))
                             : base(visualState, caller)
        { }
    }
}
