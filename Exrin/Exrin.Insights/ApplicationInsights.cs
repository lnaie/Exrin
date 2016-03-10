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

        public void Clear(List<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public List<IInsightData> GetQueue()
        {
            throw new NotImplementedException();
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

        public void TrackCrash()
        {
            throw new NotImplementedException();
        }

        public void TrackEvent(string objectName, string message)
        {
            throw new NotImplementedException();
        }

        public void TrackException()
        {
            throw new NotImplementedException();
        }

        public void TrackMetric()
        {
            throw new NotImplementedException();
        }
    }
}
