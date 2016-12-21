namespace Exrin.Abstraction
{
    public struct Tuple<T1, T2>
    {
        public readonly T1 Key;
        public readonly T2 Platform;
        public Tuple(T1 key, T2 platform) { Key = key; Platform = platform; }
    }

    public static class Tuple
    {
        public static Tuple<T1, T2> Create<T1, T2>(T1 key, T2 platform)
        {
            return new Tuple<T1, T2>(key, platform);
        }
    }
}
