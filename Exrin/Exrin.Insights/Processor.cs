using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Insights
{
    public class Processor : IInsightsProcessor
    {
        private Dictionary<string, IInsightsProvider> _insightProviders = new Dictionary<string, IInsightsProvider>();

        private readonly IApplicationInsights _applicationInsights = null;
        private object _lock = new object();
        private object _runningLock = new object();
        private Timer _timer = null;

        public Processor(IApplicationInsights applicationInsights)
        {
            _applicationInsights = applicationInsights;
        }

        public void Start(int tickIntervalMilliseconds)
        {
            var state = new object();

            // TODO: Remove timer and setup a Blocking Queue, with possible wait for sending to external (flag on external and delayed grouping before send to conserve battery / minor data usage)
            _timer = new Timer(async (msg) => { await ProcessData(msg); }, state, tickIntervalMilliseconds, tickIntervalMilliseconds);
        }

        public void Stop()
        {
            _timer.Cancel();
         
        }

        private bool isRunning = false;

        /// <summary>
        /// Will get the data from application insights and send them to the appropriate place
        /// </summary>
        /// <param name="state"></param>
        private async Task ProcessData(object state)
        {
            lock (_runningLock)
            {
                if (isRunning)
                    return;
                isRunning = true;
            }

            var list = await _applicationInsights.GetQueue();

            foreach (var item in list)
                foreach (var provider in _insightProviders.Values)
                    if (provider != null)
                        await provider.Send(item);

            await _applicationInsights.Clear(list);

            isRunning = false;
        }

        public void DeregisterService(string id)
        {
            lock (_lock)
                if (_insightProviders.ContainsKey(id))
                    _insightProviders.Remove(id);

        }

        public void RegisterService(string id, IInsightsProvider provider)
        {
            lock (_lock)
            {
                if (_insightProviders.ContainsKey(id))
                    throw new Exception($"{id} has already been added to registered Insight Services");

                _insightProviders.Add(id, provider);
            }
        }

        public void Dispose()
        {
            Stop();

            _timer.Dispose();
        }
        
    }
}
