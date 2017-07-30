using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface ITypeDefinition
    {
        Type Type { get; }

        bool NoHistory { get; }

        bool CacheView { get; }
    }
}
