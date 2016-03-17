using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework.Tests.ViewModelExecute.Mocks
{
    public class Injection : IInjection
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

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void Register<T>() where T : class
        {
            throw new NotImplementedException();
        }

        void IInjection.Register<I, T>()
        {
            throw new NotImplementedException();
        }
    }
}
