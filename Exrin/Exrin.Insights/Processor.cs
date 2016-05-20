using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Exrin.Insights
{
    public class Processor : IInsightsProcessor
    {
        private Dictionary<string, IInsightsProvider> _insightProviders = new Dictionary<string, IInsightsProvider>();

        private readonly IApplicationInsights _applicationInsights = null;
        private object _lock = new object();
        private object _runningLock = new object();
        private CancellationTokenSource _tokenSource = null;

        public Processor(IApplicationInsights applicationInsights)
        {
            _applicationInsights = applicationInsights;
        }

        public void Start(int tickIntervalMilliseconds)
        {
            var state = new object();

            _tokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                do
                {
                    try
                    {
                        var blockingQueue = await _applicationInsights.GetQueue();

                        var data = blockingQueue.Dequeue();

                        foreach (var provider in _insightProviders.Values)
                            if (provider != null)
                                await provider.Process(data);

                    }
                    catch
                    {
                        await Task.Delay(5000); // Pause to avoid thrashing if continual failure
                    }

                } while (true);

            }, _tokenSource.Token);

        }

        public void Stop()
        {
            _tokenSource.Cancel();
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

            _tokenSource = null;
        }

    }
}
