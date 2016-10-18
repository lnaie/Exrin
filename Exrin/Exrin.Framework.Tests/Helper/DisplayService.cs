using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework.Tests.Helper
{
    public class DisplayService : IDisplayService
    {
        private INavigationProxy _page = null;
        public void Init(INavigationProxy page)
        {
            _page = page;
        }

        public async Task ShowDialog(string title, string message)
        {
            await _page.ShowDialog(new DialogOptions() { Title = title, Message = message });
        }
    }
}
