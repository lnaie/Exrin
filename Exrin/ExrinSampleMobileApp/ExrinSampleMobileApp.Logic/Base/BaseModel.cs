using Exrin.Abstraction;

namespace ExrinSampleMobileApp.Logic.Base
{
    public class BaseModel : Exrin.Framework.Model
    {
        public BaseModel(IExrinContainer exrinContainer, IModelState modelState)
            : base(exrinContainer, modelState)
        {
        }
    }
}
