namespace Exrin.Abstraction
{
    using System;

    [Flags]
    public enum StackResult
    {
        None = 1,
        ArgsPassed = 2,
        StackStarted = 4,
        NavigationStarted = 8
    }
}
