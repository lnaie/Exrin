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

        // Scenarios
        //
        // DeviceInfo null
        // InsightStorage failure
        // Application Insights gives back what you put in
        // Insight Data has correct default values
        // Memory insight storage
        // 


        [Fact]
        public async Task DeviceInfoExceptionHandlingDefaults()
        {
            var startTime = DateTime.UtcNow;

            var ai = BuildService.GetInsights();

            await ai.TrackEvent("Test", "Message");

            var data = (await ai.GetQueue()).Dequeue();

            
            Assert.Equal(true, data.Created > startTime);
            Assert.Equal("0.0.0.0", data.AppVersion.ToString());
            Assert.Equal(null, data.Battery);
           // Assert.Equal(nameof(DeviceInfoExceptionHandling), data[0].CallerName);
            Assert.Equal(InsightCategory.Event, data.Category);
            Assert.Equal(null, data.ConnectionStrength);
            Assert.Equal(ConnectionType.Unknown, data.ConnectionType);
            Assert.Equal("Test", data.CustomMarker);
            Assert.Equal(null, data.CustomValue);
            Assert.Equal("", data.DeviceIdentifier);
            Assert.Equal("", data.IPAddress);
            Assert.Equal("Message", data.Message);
            Assert.Equal("", data.Model);
            Assert.Equal("0.0.0.0", data.OSVersion.ToString());
            Assert.Equal("", data.StackTrace);

        }

    }
}
