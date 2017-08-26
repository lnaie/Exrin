using Exrin.Abstraction;
using Exrin.Framework;
using Xamarin.Forms;

namespace ExrinSampleMobileApp
{
	using ExrinSampleMobileApp.Framework.Locator;

	public partial class App : Application
	{
		public App(IPlatformBootstrapper platform)
		{
			InitializeComponent();
			
			// Intializes everything and sets the MainPage to the navigation option set.
			var navService = Bootstrapper.GetInstance()
						.Init()
						.Get<INavigationService>();

			navService.Navigate(new StackOptions()
			{
				StackChoice = Stacks.Main
			});

			//var inspector = new Exrin.Inspector.Inspector(navService);

			//Task.Run(() => inspector.Init("10.0.2.15", 8888));

		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
