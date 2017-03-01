namespace ExrinSampleMobileApp.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new ExrinSampleMobileApp.App(new Bootstrapper()));
        }
    }
}
