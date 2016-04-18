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
        private INavigationContainer _container = null;
        public void Init(INavigationContainer container)
        {
            _container = container;
        }

        public async Task ShowDialog(string title, string message)
        {
            await _container.ShowDialog(new DialogOptions() { Title = title, Message = message });
        }
    }
}
