using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ExrinInspector
{
	/// <summary>
	/// Interaction logic for Connect.xaml
	/// </summary>
	public partial class Connect : Page
	{
		public Connect()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var cancellationToken = new CancellationTokenSource(5000);

			var ip = IPAddressTextBlock.Text;
			var port = PortTextBlock.Text;

			Task.Run(async () =>
			{
				var app = (Application.Current as App);

				App.Client = new TcpClient();

				App.Client.Connect(new IPEndPoint(IPAddress.Parse(ip), Convert.ToInt32(port)));

				await Task.Factory.StartNew(async () => { await app.MonitorStream(); });			

				Dispatcher.Invoke(() =>
				{
					NavigationService.GetNavigationService(this).Navigate(new VisualState());
				});

			}, cancellationToken.Token).ContinueWith((t) =>
			{
				if (t.Exception != null)
					MessageBox.Show(t.Exception.InnerException.Message);
				else if (cancellationToken.IsCancellationRequested)
					MessageBox.Show("Timeout!");

			});
		}

	
	}
}
