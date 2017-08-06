using Exrin.Abstraction;
using Exrin.Framework;
using ExrinSampleMobileApp.Framework.Locator;
using System;
using System.Threading.Tasks;

namespace ExrinSampleMobileApp.Isolate1.ViewModel
{
	public class MainViewModel : Exrin.Framework.ViewModel
    {
        public MainViewModel()
        {
        }

        public override Task OnNavigated(object args)
        {
            return base.OnNavigated(args);
        }

        public override Task OnBackNavigated(object args)
        {
            return base.OnBackNavigated(args);
        }

	}

}
