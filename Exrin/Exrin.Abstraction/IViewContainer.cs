using System.Collections.Generic;

namespace Exrin.Abstraction
{
    public interface IViewContainer
    {
        string Identifier { get; }
        object NativeView { get; }
		IDictionary<object, ContainerType> RegionMapping { get; }
    }
}
