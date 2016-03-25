using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IStackRunner
    {

        void RegisterStack<T>(object stackChoice) where T : class, IStack;

        void Run(object stackChoice, object args = null);

    }
}
