using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Exrin.Common
{
    public class AssemblyHelper
    {

        public static IList<TypeInfo> GetTypes(Type appType, Type @interface)
        {
            var query = from t in appType.GetTypeInfo().Assembly.DefinedTypes
                        where t.IsClass && !t.IsSealed && t.ImplementedInterfaces.Any(x=> x == @interface)
                        select t;

            return query.ToList();
        }

    }
}
