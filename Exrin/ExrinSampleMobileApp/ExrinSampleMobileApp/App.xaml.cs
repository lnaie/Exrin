using Exrin.Abstraction;
using Exrin.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ExrinSampleMobileApp
{
	using Framework.Locator;
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
				await inspector.Init("127.0.0.1", 8888);
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
