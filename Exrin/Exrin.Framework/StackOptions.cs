namespace Exrin.Framework
{
    using System;
    using System.Collections.Generic;
    using Abstraction;

    public class StackOptions : IStackOptions
    {
        public object Args { get; set; }

        public string ArgsKey { get; set; }

        public IDictionary<string, object> PredefinedStack { get; set; }
    }
}
