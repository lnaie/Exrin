namespace Exrin.Framework
{
    using Abstraction;
    using System.Collections.Generic;

    public class StackOptions : IStackOptions
    {

        public object StackChoice { get; set; }

        public string ViewKey { get; set; }

        public object Args { get; set; }

        public string ArgsKey { get; set; }

        public bool NewInstance { get; set; } = false;

        public IDictionary<string, object> PredefinedStack { get; set; }
    }
}
