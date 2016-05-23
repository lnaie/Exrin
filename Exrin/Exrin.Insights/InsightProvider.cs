using Exrin.Abstraction;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Exrin.Insights
{
    public abstract class InsightProvider : IInsightsProvider
    {

        private readonly IInsightStorage _storage = null;
        private readonly int _tick = 0;
        public InsightProvider(IInsightStorage storage, int tick = 300000) // Default 5 minutes
        {
            _storage = storage;
            _tick = tick;

            // Immediate Process and Start Timer
            if (tick > 0)
                Task.Run(Process);
        }

        private static object _lock = new object();
        private bool _isRunning = false;

        private Task Process()
        {
            var state = new object();
            new Timer(async (s) =>
            {
                lock (_lock)
                {
                    if (_isRunning)
                        return;

                    _isRunning = true;
                }
                try
                {
                    foreach (var data in await _storage.ReadAllData())
                        if (await Send(data))
                            await _storage.Delete(data);
                }
                catch(Exception ex) { Debug.WriteLine(ex.Message); }
                finally
                {
                    _isRunning = false;
                }
            }, state, 0, _tick, true); // Instant first run

            return Task.FromResult(true);
        }

     
        /// <summary>
        /// Will send the insight data to the insight provider
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected abstract Task<bool> Send(IInsightData data);

        /// <summary>
        /// Saves to storage for later processing.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual async Task Record(IInsightData data)
        {
            await _storage.Write(data);
        }
    }
}
