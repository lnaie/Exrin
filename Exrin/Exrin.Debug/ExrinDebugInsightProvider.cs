namespace Exrin.Debug
{
    using Abstraction;
    using Insights;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ExrinDebugInsightProvider : InsightProvider
    {
        // IInsight Storage - Output direct to Visualizer
        public ExrinDebugInsightProvider(IInsightStorage storage) : base(storage, 60000) { }

        /// <summary>
        /// We are not sending this data anywhere. Using the IInsightStorage to output direct to visualizer
        /// </summary>
        /// <param name="insights"></param>
        /// <returns></returns>
        protected override Task<IList<IInsightData>> Send(IList<IInsightData> insights)
        {
            IList<IInsightData> success = new List<IInsightData>();

            foreach (var data in insights)
                success.Add(data);

            return Task.FromResult(success);
        }
    }
}