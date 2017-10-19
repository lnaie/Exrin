using Exrin.Abstraction;
using Exrin.Framework;
using Xunit;

namespace Exrin.Tests
{
	public class NavigationServiceTests
    {

		[Fact]
		public void Test1()
		{
			// Need to Mock
			IViewService viewService = null;
			INavigationState state = null;
			IInjectionProxy injection = null;
			IDisplayService displayService = null;

			var navigationService = new NavigationService(viewService, state, injection, displayService);

			// Need to create and possibly Mock?
			IStack stack = null;
			navigationService.RegisterStack(stack);
			navigationService.Init((p) => { }, () => { return new object(); });
		}


    }
}
