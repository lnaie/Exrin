using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class DisplayService : IDisplayService
    {

        public void Init(INavigationPage page)
        {
            //throw new NotImplementedException();
        }

        public Task ShowDialog(string message)
        {
            //throw new NotImplementedException();
            return Task.FromResult(0);
        }
    }
}
