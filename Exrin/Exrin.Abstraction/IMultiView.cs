namespace Exrin.Abstraction
{
    using System.Collections.Generic;

    public interface IMultiView: IView
    {
        IList<IView> Views { get; } 
    }
}
