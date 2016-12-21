namespace Exrin.Framework
{
    using Abstraction;
    using Common;
    using System;
    using System.Threading;

    public static class App
    {
        // TODO: Refactor, instead of static instance, should be injected
        internal static IPlatformOptions PlatformOptions = new PlatformOptions();

        [Obsolete("Please use Init(IPlatformOptions options)")]
        public static void Init()
        {
            Init(SynchronizationContext.Current);
        }

        public static void Init(IPlatformOptions options)
        {
            Init(SynchronizationContext.Current, options);
        }

        [Obsolete("Please use Init(SynchronizationContext uiContext, IPlatformOptions options)")]
        /// <summary>
        /// Will initialize anything needed within the framework.
        /// </summary>
        /// <param name="uiContext">Example: Pass through SynchronizationContext.Current</param>
        public static void Init(SynchronizationContext uiContext)
        {
            ThreadHelper.Init(uiContext);
        }

        public static void Init(SynchronizationContext uiContext, IPlatformOptions options)
        {
            ThreadHelper.Init(uiContext);
            PlatformOptions = options;
        }

    }
}
