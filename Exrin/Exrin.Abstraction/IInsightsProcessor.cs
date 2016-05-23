using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IInsightsProcessor: IDisposable
    {
        void Start();
        void Stop();
        void RegisterService(string id, IInsightsProvider provider);
        void DeregisterService(string id);

    }
}
