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

    public partial class App : Application
    {
        public App(IPlatformBootstrapper platform)
        {
            InitializeComponent();

            // Intializes everything and sets the MainPage to the navigation option set.
            Bootstrapper.GetInstance()
                        .Init()
                        .Get<INavigationService>()
                        .Navigate(new StackOptions()
                        {
                            StackChoice = Stacks.Authentication
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
