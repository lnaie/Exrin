namespace Exrin.Abstraction
{
    public interface IVisualizer
    {

        void Initialize(IVisualState visualState);

        void AddInsight(IInsightData data);

    }
}
