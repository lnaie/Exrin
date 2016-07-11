namespace Exrin.Framework
{

    using Common;
    using System.Threading;

    public static class App
    {

        public static void Init()
        {
            Init(SynchronizationContext.Current); 
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
