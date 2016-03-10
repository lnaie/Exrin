using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Insights
{
    public class Process
    {
        private readonly IApplicationInsights _insights = null;
        private readonly IDeviceInfo _deviceInfo = null;

        public Process(IApplicationInsights insights, IDeviceInfo deviceInfo)
        {
            _insights = insights;
            _deviceInfo = deviceInfo;
        }
       

        // TODO: Hook event in Application Insights and store to local file
        // Then have a timer started that will process the data and send to the appropriate place (if appropriate)
        // E.g. if I attached insights, they deal with that, but others might have an API we need to manually call.

    }
}
