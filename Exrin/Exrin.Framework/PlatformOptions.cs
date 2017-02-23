namespace Exrin.Framework
{
    using Abstraction;
    public class PlatformOptions : IPlatformOptions
    {
        public string Platform { get; set; } = null;
        public bool StateTracking { get; set; } = false;
    }
}
