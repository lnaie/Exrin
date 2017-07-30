namespace Exrin.Framework
{
    using Abstraction;
    using System.Collections.Generic;
    using System.Reflection;

    public static class Extensions
    {
        public static IList<IResult> ToList(this Result result)
        {
            return new List<IResult>() { result };
        }

        public static AssemblyName GetAssemblyName(object instance)
        {
            return instance.GetType().GetTypeInfo().Assembly.GetName();
        }
    }
}
