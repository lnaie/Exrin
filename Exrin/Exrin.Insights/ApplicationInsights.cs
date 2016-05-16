using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Exrin.Common;

namespace Exrin.Insights
{
    public class ApplicationInsights : IApplicationInsights
    {
        private readonly IInsightStorage _storage = null;
        private readonly INavigationReadOnlyState _navigationState = null;
        private readonly IDeviceInfo _deviceInfo = null;
        private string _userId = null;
        private string _fullName = null;
        private static string _sessionId = Guid.NewGuid().ToString(); // Once per application load

        public ApplicationInsights(IInsightStorage storage, IDeviceInfo deviceInfo, INavigationReadOnlyState navigationState)
        {
            _storage = storage;
            _deviceInfo = deviceInfo;
            _navigationState = navigationState;
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
            data.Created = DateTime.UtcNow;
            data.AppVersion = DefaultHelper.GetOrDefault(_deviceInfo.GetAppVersion, new Version("0.0.0.0"));
            data.Battery = await DefaultHelper.GetOrDefaultAsync(_deviceInfo.GetBattery, null);
            data.ConnectionStrength = await DefaultHelper.GetOrDefaultAsync(_deviceInfo.GetConnectionStrength, null);
            data.ConnectionType = DefaultHelper.GetOrDefault(_deviceInfo.GetConnectionType, ConnectionType.Unknown);
            data.DeviceIdentifier = DefaultHelper.GetOrDefault(_deviceInfo.GetUniqueId, "");
            data.FullName = _fullName;
            data.Id = Guid.NewGuid();
            data.IPAddress = await DefaultHelper.GetOrDefaultAsync(_deviceInfo.GetIPAddress, "");
            data.Model = await DefaultHelper.GetOrDefaultAsync(_deviceInfo.GetModel, "");
            data.OSVersion = await DefaultHelper.GetOrDefaultAsync(_deviceInfo.GetOSVersion, new Version("0.0.0.0"));
            data.SessionId = _sessionId;
            data.UserId = _userId;

            // Fill State
        }

        public async Task TrackMetric(string category, object value, [CallerMemberName] string callerName = "")
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task TrackException(Exception exception, [CallerMemberName] string callerName = "")
        {
            try
            {
                var data = new InsightData()
                {
                    Category = InsightCategory.Exception,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    CallerName = callerName
                };

                await FillData(data);
                Store(data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task TrackEvent(string eventName, string message, [CallerMemberName] string callerName = "")
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Store event in file for transmission
        /// </summary>
        /// <param name="data"></param>
        private void Store(IInsightData data)
        {
            try
            {
                _storage?.Write(data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public Task TrackRaw(IInsightData data)
        {
            try
            {
                _storage.Write(data);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Task.FromResult(false);
            }
        }
    }
}
