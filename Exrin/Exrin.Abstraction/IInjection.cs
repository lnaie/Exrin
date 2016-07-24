namespace Exrin.Abstraction
{
    using System;

    public interface IInjection
    {

        void Init();

        void Complete();

        bool IsRegistered<T>();

        void Register<T>(InstanceType type = InstanceType.SingleInstance) where T : class;


        void RegisterInterface<I, T>(InstanceType type = InstanceType.SingleInstance) where T : class, I
                                             where I : class;

        void RegisterInstance<I, T>(T instance) where T : class, I
                                             where I : class;

		void RegisterInstance<I>(I instance) where I : class;

		T Get<T>(bool optional = false) where T : class;

        object Get(Type type);


    }
}
