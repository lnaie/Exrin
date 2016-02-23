using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework.Tests.Builder
{
    /// <summary>
    /// This Builder needs to be implemented locally in the project
    /// Due to its current design. See if this can be refactored into the package
    /// </summary>
    public class ViewModelExecuteBuilder
    {
        private readonly IApplicationInsights _insights;

        public ViewModelExecuteBuilder(IApplicationInsights insights)
        {
            _insights = insights;
        }
        
        public IViewModelExecute BuildNew()
        {
            return 
        }

    }
}
