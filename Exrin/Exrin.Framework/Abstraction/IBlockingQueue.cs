using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IBlockingQueue<T>: IEnumerable<T>
    {
        T Dequeue();
        IList<T> DequeueAll();
        void Enqueue(T data);
    }
}
