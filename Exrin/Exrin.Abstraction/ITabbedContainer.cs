namespace Exrin.Abstraction
{
    using System.Collections.Generic;

    public interface ITabbedContainer : IViewContainer
    {
        ITabbedView TabbedView { get; set; }
        IList<IStack> Children { get; set; }

    }
}
