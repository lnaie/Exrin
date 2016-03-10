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

        void Register<T>() where T : class;
      

        void Register<I, T>() where T : class, I
                                             where I : class;

        
        T Get<T>() where T : class;

        object Get(Type type);


    }
}
