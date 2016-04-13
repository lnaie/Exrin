using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Insights
{
    public class ApplicationInsights : IApplicationInsights
    {
        private readonly IInsightStorage _storage = null;
        private readonly IDeviceInfo _deviceInfo = null;
        private string _userId = null;
        private string _fullName = null;
        private static Guid _sessionId = Guid.NewGuid(); // Once per application load
        
        public ApplicationInsights(IInsightStorage storage, IDeviceInfo deviceInfo)
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
        private async Task FillData(IInsightData data)
        {
            data.Added = DateTime.UtcNow;
            data.AppVersion = _deviceInfo.GetAppVersion();
            data.Battery = await _deviceInfo.GetBattery();
            data.ConnectionStrength = await _deviceInfo.GetConnectionStrength();
            data.ConnectionType = _deviceInfo.GetConnectionType();
            data.DeviceIdentifier = _deviceInfo.GetUniqueId();
            data.FullName = _fullName;
            data.Id = Guid.NewGuid();
            data.IPAddress = await _deviceInfo.GetIPAddress();
            data.Model = await _deviceInfo.GetModel();
            data.OSVersion = await _deviceInfo.GetOSVersion();
            data.SessionId = _sessionId;
            data.UserId = _userId;
        }
        
        public async Task TrackMetric(string category, object value, [CallerMemberName] string callerName = "")
        {
            var data = new InsightData()
            {
                Category = InsightCategory.Metric,
                CustomMarker = category,
                CustomValue = value,
                CallerName = callerName
            };

            await FillData(data);
            Store(data);
        }

        public async Task TrackException(Exception ex, [CallerMemberName] string callerName = "")
        {
            var data = new InsightData()
            {
                Category = InsightCategory.Exception,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                CallerName = callerName
            };

            await FillData(data);
            Store(data);
        }

        public async Task TrackEvent(string eventName, string message, [CallerMemberName] string callerName = "")
        {
            var data = new InsightData()
            {
                Category = InsightCategory.Event,
                Message = message,
                CustomMarker = eventName,
                CallerName = callerName
            };

            await FillData(data);
            Store(data);
        }

        /// <summary>
        /// Store event in file for transmission
        /// </summary>
        /// <param name="data"></param>
        private void Store(IInsightData data)
        {
            _storage.Write(data);
        }

        public Task TrackRaw(IInsightData data)
        {
            _storage.Write(data);
            return Task.FromResult(true);
        }
    }
}
