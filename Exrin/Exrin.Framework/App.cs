namespace Exrin.Framework
{
    using Common;
    using System.Threading;

    public static class App
    {
        internal static bool IsDebugging { get; set; } = false;
        public static void Init(bool debugMode = false)
        {
            Init(SynchronizationContext.Current);
            IsDebugging = debugMode;
        }

        /// <summary>
        /// Will initialize anything needed within the framework.
        /// </summary>
        /// <param name="uiContext">Example: Pass through SynchronizationContext.Current</param>
        public static void Init(SynchronizationContext uiContext)
        {
            ThreadHelper.Init(uiContext);
        }

    }
}
