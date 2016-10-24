namespace Exrin.Framework
{
    using Abstraction;
    using System;

    public class MapOptions : IMapOptions
    {
        public bool CacheView { get; set; } = false;

        public bool NoHistory { get; set; } = false;
    }
}
