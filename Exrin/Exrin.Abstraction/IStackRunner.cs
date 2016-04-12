using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IStackRunner
    {
        void Init(Action<object> setRoot);

        void RegisterStack<T>() where T : class, IStack;

        void Run(object stackChoice, object args = null);
    }
}
