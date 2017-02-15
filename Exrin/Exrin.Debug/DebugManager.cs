namespace Exrin.Debug
{
    using Abstraction;

    public class DebugManager: IDebugManager
    {
        private readonly IVisualizer _visualizer = null;
        public DebugManager(IVisualizer visualizer)
        {
            _visualizer = visualizer;
        }
        
        public void AddInsight(IInsightData data)
        {
            _visualizer?.AddInsight(data);
        }
    }
}
