using Exrin.Abstraction;
using System.Threading.Tasks;

namespace ExrinSampleMobileApp.Framework.Abstraction.Model
{
    public interface IAuthModel : IBaseModel
    {
        Task<bool> Login();
    }
}
