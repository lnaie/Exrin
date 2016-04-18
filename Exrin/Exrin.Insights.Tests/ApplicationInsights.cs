using Exrin.Abstraction;
using Exrin.Insights.Tests.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Exrin.Insights.Tests
{
    public class ApplicationInsights
    {

        [Fact]
        public async Task DeviceInfoExceptionHandling()
        {
            var startTime = DateTime.UtcNow;

            var ai = BuildService.GetInsights();

            await ai.TrackEvent("Test", "Message");

            var data = await ai.GetQueue();

            Assert.Equal(1, data.Count);

            Assert.Equal(true, data[0].Added > startTime);
            Assert.Equal("0.0.0.0", data[0].AppVersion.ToString());
            Assert.Equal(null, data[0].Battery);
           // Assert.Equal(nameof(DeviceInfoExceptionHandling), data[0].CallerName);
            Assert.Equal(InsightCategory.Event, data[0].Category);
            Assert.Equal(null, data[0].ConnectionStrength);
            Assert.Equal(ConnectionType.Unknown, data[0].ConnectionType);
            Assert.Equal("Test", data[0].CustomMarker);
            Assert.Equal(null, data[0].CustomValue);
            Assert.Equal("", data[0].DeviceIdentifier);
            Assert.Equal("", data[0].IPAddress);
            Assert.Equal("Message", data[0].Message);
            Assert.Equal("", data[0].Model);
            Assert.Equal("0.0.0.0", data[0].OSVersion.ToString());
            Assert.Equal("", data[0].StackTrace);

        }

    }
}
