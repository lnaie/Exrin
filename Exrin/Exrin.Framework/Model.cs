using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class Model : BindableObject, IModel
    {
        
        public IModelExecution Execution { get; protected set; }

    }
}
