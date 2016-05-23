using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IInsightsProvider
    {
        /// <summary>
        /// Sends the insights data to the provider
        /// </summary>
        /// <param name="data"></param>
        Task Record(IInsightData data);

    }
}
