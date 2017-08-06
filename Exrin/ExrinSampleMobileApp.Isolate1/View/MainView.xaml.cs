using System;
using Exrin.Abstraction;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExrinSampleMobileApp.Isolate1.View
{
	public partial class MainView : ContentPage, IView
	{
		public MainView ()
		{
			InitializeComponent ();
		}

		protected override bool OnBackButtonPressed()
		{
			return ((IView)this).OnBackButtonPressed();
		}

		Func<bool> IView.OnBackButtonPressed { get; set; }
	}
}