namespace Exrin.Framework
{
    using Abstraction;
    using System.Collections.Generic;
    
    public static class Extensions
    {
        public static IList<IResult> ToList(this Result result)
        {
            return new List<IResult>() { result };
        }
    }
}
