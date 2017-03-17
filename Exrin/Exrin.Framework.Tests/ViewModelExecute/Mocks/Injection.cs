namespace Exrin.Framework.Tests.ViewModelExecute.Mocks
{
    using Exrin.Abstraction;
    using System;

    public class Injection : IInjectionProxy
    {
        public void Complete()
        {
            throw new NotImplementedException();
        }

        public object Get(Type type)
        {
            throw new NotImplementedException();
        }

        public T Get<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public T Get<T>(bool optional = false) where T : class
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered<T>()
        {
            throw new NotImplementedException();
        }

        public void Register<T>(InstanceType type) where T : class
        {
            throw new NotImplementedException();
        }

        public void RegisterInstance<I>(I instance) where I : class
        {
            throw new NotImplementedException();
        }

        void IInjectionProxy.RegisterInstance<I, T>(T instance)
        {
            throw new NotImplementedException();
        }

        void IInjectionProxy.RegisterInterface<I, T>(InstanceType type)
        {
            throw new NotImplementedException();
        }
    }
}
