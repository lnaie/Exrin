using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IInsightStorage
    {

        Task Write(IInsightData data);

        Task<IList<IInsightData>> ReadAllData();

        Task Delete(IInsightData data);

    }
}
