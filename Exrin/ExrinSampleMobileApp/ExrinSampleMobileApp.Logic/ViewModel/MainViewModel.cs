using Exrin.Abstraction;
using ExrinSampleMobileApp.Framework.Abstraction.Model;
using ExrinSampleMobileApp.Logic.Base;
using ExrinSampleMobileApp.Logic.VisualState;

namespace ExrinSampleMobileApp.Logic.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel(IMainModel model, IExrinContainer exrinContainer)
            : base(exrinContainer, new MainVisualState(model))
        {
        }
    }
}
