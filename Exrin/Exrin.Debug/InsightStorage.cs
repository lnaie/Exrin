namespace Exrin.Debug
{
    using Abstraction;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class InsightStorage : IInsightStorage
    {
        private readonly IDebugManager _debugManager = null;
        public InsightStorage(IDebugManager debugManager)
        {
            _debugManager = debugManager;
        }

        public Task Delete(IInsightData data)
        {
            return Task.FromResult(true);
        }

        public Task<IList<IInsightData>> ReadAllData()
        {
            IList<IInsightData> list = new List<IInsightData>();
            return Task.FromResult(list);
        }

        public Task Write(IInsightData data)
        {
            _debugManager.AddInsight(data);
            return Task.FromResult(true);
        }
    }
}
