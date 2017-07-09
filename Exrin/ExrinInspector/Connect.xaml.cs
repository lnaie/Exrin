using Exrin.Inspector;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

		private void button_Click(object sender, RoutedEventArgs e)
		{
			var cancellationToken = new CancellationTokenSource(5000);

			var ip = IPAddressTextBlock.Text;
			var port = PortTextBlock.Text;

			Task.Run(() =>
			{
				TcpClient client = new TcpClient();

				client.Connect(new IPEndPoint(IPAddress.Parse(ip), Convert.ToInt32(port)));

				var stream = client.GetStream();

				// TEST CODE
				var command = new Command() { Type = CommandType.VisualState };
				var data = JsonConvert.SerializeObject(command) + Inspector.EOT;
				WriteString(stream, data);

				NavigationService.GetNavigationService(this).Navigate(new VisualState());

			}, cancellationToken.Token).ContinueWith((t) =>
			{
				if (t.Exception != null)
					MessageBox.Show(t.Exception.InnerException.Message);
				else if (cancellationToken.IsCancellationRequested)
					MessageBox.Show("Timeout!");
				else
					MessageBox.Show("Connected!");
			});
		}

		private void WriteString(NetworkStream stream, string text)
		{
			var byteArray = Encoding.UTF8.GetBytes(text);

			stream.Write(byteArray, 0, byteArray.Length);
		}
	}
}
