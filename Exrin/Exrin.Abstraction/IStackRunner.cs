namespace Exrin.Abstraction
{
    using System;

    public interface IStackRunner
    {
        void Init(Action<object> setRoot);

        void RegisterViewContainer<T>() where T : class, IViewContainer;

        StackResult Run(object stackChoice, IStackOptions options = null);

        void Rebuild();

        object CurrentStack { get; }
    }
}
