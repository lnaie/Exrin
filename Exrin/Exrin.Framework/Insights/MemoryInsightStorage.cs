namespace Exrin.Insights
{
    using Abstraction;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MemoryInsightStorage : IInsightStorage
    {

        private IList<IInsightData> _storage = new List<IInsightData>();

        public Task Delete(IInsightData data)
        {
            _storage.Remove(_storage.First(d => d.Id == data.Id));
            return Task.FromResult(true);
        }

        public Task<IList<IInsightData>> ReadAllData()
        {
            return Task.FromResult(_storage);
        }

        public Task Write(IInsightData data)
        {
            _storage.Add(data);
            return Task.FromResult(true);
        }
    }
}
