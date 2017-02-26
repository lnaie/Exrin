namespace Exrin.Abstraction
{
    using System.Collections.Generic;

    public interface IStackOptions
    {

        object StackChoice { get; }

        object Args { get; }

        bool NewInstance { get; set; }

        /// <summary>
        /// An ordered array with the Page Keys for a stack to be preloaded.The last page on the array will be the visible one.
        /// </summary>
        IDictionary<string, object> PredefinedStack { get; }

        /// <summary>
        /// If the ArgsKey is blank pass the args through. Otherwise the ArgsKey must match the PageKey being loaded for the args to be passed.
        /// </summary>
        string ArgsKey { get; }

        string ViewKey { get; set; }
    }
}
