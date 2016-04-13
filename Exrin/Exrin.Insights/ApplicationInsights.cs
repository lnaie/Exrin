using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Insights
{
    public class ApplicationInsights : IApplicationInsights
    {
        private readonly ILocalStorage _storage = null;
        private readonly IDeviceInfo _deviceInfo = null;
        private string _userId = null;
        private string _fullName = null;
        private Guid _sessionId = Guid.NewGuid(); // Once per application load

        public ApplicationInsights(ILocalStorage storage, IDeviceInfo deviceInfo)
        {
            _storage = storage;
            _deviceInfo = deviceInfo;
        }

        public async Task Clear(IList<IInsightData> list)
        {
            foreach (var data in list)
               await _storage.Delete(data);
        }

        public async Task<List<IInsightData>> GetQueue()
        {
            return (await _storage.ReadAllData()).ToList();
        }

        public void SetIdentity(string userId, string fullName)
        {
            _userId = userId;
            _fullName = fullName;
        }

        /// <summary>
        /// Used to fill in the extra details into the insights data before storage.
        /// </summary>
        /// <param name="data"></param>
        private void FillData(IInsightData data)
        {

        }
        
        public Task TrackMetric(string metricIdentifier, object value, string key = "")
        {
            throw new NotImplementedException();
        }

        public Task TrackException(Exception ex)
        {
            throw new NotImplementedException();
        }

        public Task TrackCrash(Exception ex)
        {
            throw new NotImplementedException();
        }

        public Task TrackEvent(string eventName, string message)
        {
            throw new NotImplementedException();
        }
    }
}
