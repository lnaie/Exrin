using Exrin.Abstraction;
using Exrin.Framework;
using Xamarin.Forms;

namespace ExrinSampleMobileApp
{
	using Framework.Locator;
	using System;
	using System.Threading.Tasks;

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
				StackChoice = Stacks.Authentication
			});

			var inspector = new Exrin.Inspector.Inspector(navService);

			Task.Run(async () =>
			{
				try
				{
					await inspector.Init("10.0.2.15", 8888);
				}
				catch (Exception ex)
				{
					var message = ex.Message;
				}
			});

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
