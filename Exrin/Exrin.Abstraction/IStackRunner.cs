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

        void RegisterViewContainer<T>() where T : class, IViewContainer;

        void Run(object stackChoice, object args = null, Dictionary<string, object> predefined = null);

        void Rebuild();

        object CurrentStack { get; }
    }
}
