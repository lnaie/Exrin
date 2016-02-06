using Xamarin.Forms;

namespace Exrin.App
{
    public class App : Application
	{
		public App ()
		{
			var button = new Button
            {
                Text = "Click Me!",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

    int clicked = 0;
    button.Clicked += (s, e) => button.Text = "Clicked: " + clicked++;

            MainPage = new ContentPage();
		}
	}
}
