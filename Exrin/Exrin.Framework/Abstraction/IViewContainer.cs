using System.Collections.Generic;

namespace Exrin.Abstraction
{
    public interface IViewContainer: IHolder
    {
        string Identifier { get; }
		object NativeView { get; }
		IDictionary<object, ContainerType> RegionMapping { get; }
		IViewContainer ParentContainer { get; }
    }
}
