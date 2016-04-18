using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Insights.Tests.Mock
{
    public class InsightStorage : IInsightStorage
    {
        public Task Delete(IInsightData data)
        {
            throw new NotImplementedException();
        }

        public Task<IList<IInsightData>> ReadAllData()
        {
            throw new NotImplementedException();
        }

        public Task Write(IInsightData data)
        {
            throw new NotImplementedException();
        }
    }
}
