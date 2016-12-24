namespace Exrin.Framework
{
    using Abstraction;

    public class MapOptions : IMapOptions
    {
        public bool CacheView { get; set; } = false;

        public bool NoHistory { get; set; } = false;

        public string Platform { get; set; } = null;
    }
}
