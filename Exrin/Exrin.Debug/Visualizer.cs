namespace Exrin.Debug
{
    using Abstraction;
    using System.Collections.Generic;

    public class Visualizer : IVisualizer
    {
        private readonly IVisualDisplay _display = null;
        public Visualizer(IVisualDisplay display)
        {
            _display = display;
        }

        public void AddInsight(IInsightData data)
        {
           // throw new NotImplementedException();
        }

        public void Initialize(IVisualState visualState)
        {
            // Reflect through all properties and add to List.
            var values = new Dictionary<string, string>();
            
            _display?.Display(visualState.GetType().Name, values);
        }
    }
}
