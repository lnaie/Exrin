using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IInjection
    {

        void Init();

        void Complete();

        void Register<T>(InstanceType type) where T : class;


        void RegisterInterface<I, T>(InstanceType type) where T : class, I
                                             where I : class;


        T Get<T>() where T : class;

        object Get(Type type);


    }
}
