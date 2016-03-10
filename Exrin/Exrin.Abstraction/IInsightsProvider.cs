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
        /// <returns>Boolean signifying success or failure of the send</returns>
        Task<bool> Send(IInsightData data);

    }
}
