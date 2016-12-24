

namespace Exrin.Abstraction
{
    public interface IMapOptions
    {
        /// <summary>
        /// Remove View from history when view not visible on Stack any more.
        /// </summary>
        bool NoHistory { get; }
        bool CacheView { get; }
        string Platform { get; }
        // Possible other options
        // PreLoad View - after startup in UI Thread, would need to ensure low priority
    }
}
