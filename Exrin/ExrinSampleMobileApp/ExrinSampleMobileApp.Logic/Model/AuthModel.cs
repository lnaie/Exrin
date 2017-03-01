using Exrin.Abstraction;
using ExrinSampleMobileApp.Framework.Abstraction.Model;
using ExrinSampleMobileApp.Logic.Base;
using ExrinSampleMobileApp.Logic.ModelState;
using System.Threading.Tasks;

namespace ExrinSampleMobileApp.Logic.Model
{
    public class AuthModel : BaseModel, IAuthModel
    {
        public AuthModel(IExrinContainer exrinContainer)
            : base(exrinContainer, new AuthModelState()) { }

        public Task<bool> Login()
        {
            return Task.FromResult(true);
        }
    }
}
