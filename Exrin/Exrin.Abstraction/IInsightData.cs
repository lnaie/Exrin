using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IInsightData
    {
        Guid Id { get; set; }

        string UserId { get; set; }

        string FullName { get; set; }

        DateTime Added { get; set; }

        string StackTrace { get; set; }

        string CallerName { get; set; }

        string Message { get; set; }

        string DeviceIdentifier { get; set; }

        ConnectionType ConnectionType { get; set; }
        
        string IPAddress { get; set; }

        string Model { get; set; }

        Version AppVersion { get; set; }

        Version OSVersion { get; set; }

        double? Battery { get; set; }

        double? ConnectionStrength { get; set; }

        Guid SessionId { get; set; }

        InsightCategory Category { get; set; }

        string CustomMarker { get; set; }

        object CustomValue { get; set; }

    }
}
