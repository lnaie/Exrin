using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Insights.Tests.Mock
{
    public class BuildService
    {
        // TODO: flag for NIE
        public static IApplicationInsights GetInsights()
        {
            return new Exrin.Insights.ApplicationInsights(new Exrin.Insights.MemoryInsightStorage(), new DeviceInfo());
        }

    }
}
