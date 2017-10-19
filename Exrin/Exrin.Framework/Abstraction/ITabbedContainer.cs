namespace Exrin.Abstraction
{
    using System.Collections.Generic;

    public interface ITabbedContainer : IViewContainer
    {
        IList<IStack> Children { get; set; }

		void SetCurrentPage(IStack stack);
    }
}
