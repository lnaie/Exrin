namespace Exrin.Framework
{
    using Abstraction;
    using System;

    public class BindingAttribute: Attribute
    {
        public BindingAttribute() { }
        public BindingAttribute(BindingType type)
        {
            BindingType = type;
        }
        public BindingType BindingType { get; set; } = BindingType.OneWay;
    }
}
