using Exrin.Abstraction;

namespace ExrinSampleMobileApp.Logic.Base
{
    public class BaseViewModel : Exrin.Framework.ViewModel
    {
        public BaseViewModel(IExrinContainer exrinContainer,
                             IVisualState visualState, string caller = nameof(BaseViewModel))
                             : base(exrinContainer, visualState, caller)
        { }
    }
}
