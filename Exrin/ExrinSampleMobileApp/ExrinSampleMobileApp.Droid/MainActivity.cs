using Android.App;
using Android.OS;
using Exrin.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace ExrinSampleMobileApp.Droid
{
    [Activity(Label = "ExrinSampleMobileApp.Droid", Theme = "@style/main", MainLauncher = true, Icon = "@drawable/icon", LaunchMode =Android.Content.PM.LaunchMode.SingleTop)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            Exrin.Framework.App.Init(new PlatformOptions() { Platform = Device.RuntimePlatform });

            LoadApplication(new App(new Bootstrapper()));
        }
    }
}