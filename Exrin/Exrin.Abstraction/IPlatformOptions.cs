namespace Exrin.Abstraction
{
    public interface IPlatformOptions
    {
        string Platform { get; }

        bool StateTracking { get; }
    }
}
