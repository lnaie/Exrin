using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface ILocalStorage
    {

        Task Save(string fileId, string data);

        Task Read(string fileId, string data);

    }
}
