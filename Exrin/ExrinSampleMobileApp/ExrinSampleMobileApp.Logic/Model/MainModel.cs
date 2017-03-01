using Exrin.Abstraction;
using ExrinSampleMobileApp.Framework.Abstraction.Model;
using ExrinSampleMobileApp.Logic.Base;
using ExrinSampleMobileApp.Logic.ModelState;

namespace ExrinSampleMobileApp.Logic.Model
{
    public class MainModel : BaseModel, IMainModel
    {
        public MainModel(IExrinContainer exrinContainer)
            : base(exrinContainer, new MainModelState()) { }
    }
}
