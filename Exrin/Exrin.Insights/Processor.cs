using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Insights
{
    public class Processor: IDisposable
    {
        private Dictionary<string, IApplicationInsights> _insightTrackers = new Dictionary<string, IApplicationInsights>();

        private readonly IInsightsProvider _insightsProvider = null;
        private object _lock = new object();
        private Timer _timer = null;

        public Processor(IInsightsProvider insightsProvider)
        {
            _insightsProvider = insightsProvider;

            StartTimer();
        }

        private void StartTimer()
        {
            var state = new object();
            
            _timer = new Timer(async (msg) => { await ProcessData(msg); }, state, 120000, 120000);
        }

        private void StopTimer()
        {
            _timer.Cancel();
            _timer.Dispose();
        }

        /// <summary>
        /// Will get the data from application insights and send them to the appropriate place
        /// </summary>
        /// <param name="state"></param>
        private async Task ProcessData(object state)
        {
            foreach (var tracker in _insightTrackers.Values)
                if (tracker != null)
                    foreach (var data in await tracker.GetQueue())
                        if (await _insightsProvider.Send(data))
                            await tracker.Clear(new List<IInsightData>() { data });
           
        }

        public void DeregisterService(string id)
        {
            lock (_lock)
                if (_insightTrackers.ContainsKey(id))
                    _insightTrackers.Remove(id);
          
        }

        public void RegisterService(string id, IApplicationInsights insights)
        {
            lock (_lock)
            {
                if (_insightTrackers.ContainsKey(id))
                    throw new Exception($"{id} has already been added to registered Insight Services");

                _insightTrackers.Add(id, insights);
            }
        }

        public void Dispose()
        {
            StopTimer();
        }

    }
}
