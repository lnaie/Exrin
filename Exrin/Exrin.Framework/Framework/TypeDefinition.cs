namespace Exrin.Framework
{
    using System;
    using Abstraction;

    public class TypeDefinition: ITypeDefinition
    {

        public Type Type { get; set; }

        public bool NoHistory { get; set; }

        public bool CacheView { get; set; }

        public string Platform { get; set; } = null;

    }
}
