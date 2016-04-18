using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Insights.Tests.Mock
{
    public class DeviceInfo : IDeviceInfo
    {
        public Version GetAppVersion()
        {
            throw new NotImplementedException();
        }

        public Task<double?> GetBattery()
        {
            throw new NotImplementedException();
        }

        public Task<double?> GetConnectionStrength()
        {
            throw new NotImplementedException();
        }

        public ConnectionType GetConnectionType()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetIPAddress()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetModel()
        {
            throw new NotImplementedException();
        }

        public string GetOS()
        {
            throw new NotImplementedException();
        }

        public Task<Version> GetOSVersion()
        {
            throw new NotImplementedException();
        }

        public Size GetScreenSize()
        {
            throw new NotImplementedException();
        }

        public string GetUniqueId()
        {
            throw new NotImplementedException();
        }

    }
}
