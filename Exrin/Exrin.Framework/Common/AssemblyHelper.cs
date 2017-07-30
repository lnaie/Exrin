using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Exrin.Common
{
    public class AssemblyHelper
    {
        public static IList<TypeInfo> GetTypes(AssemblyName name, Type @interface)
        {
            //TODO: Remove LINQ to increase perf
            var query = from t in Assembly.Load(new AssemblyName(name.FullName)).DefinedTypes
                        where t.IsClass && !t.IsSealed && t.ImplementedInterfaces.Any(x => x == @interface)
                        select t;

            return query.ToList();
        }

        public static IList<TypeInfo> GetTypes(Type appType, Type @interface)
        {
           
            var query = from t in appType.GetTypeInfo().Assembly.DefinedTypes
                        where t.IsClass && !t.IsSealed && t.ImplementedInterfaces.Any(x=> x == @interface)
                        select t;

            return query.ToList();
        }

    }
}
