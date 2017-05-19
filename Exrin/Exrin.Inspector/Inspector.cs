using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Exrin.Inspector
{
	public class Inspector : IInspector
	{
		private readonly INavigationService _navigationService;

		public Inspector(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		public async Task<bool> Init(string host, int port)
		{
			await Task.Run(() =>
			RunServer(host, port)
			);

			return true;

			// Get Current Stack, Current NavigationStack, CurrentPage, CurrentBindingContext, convert to IViewModel, get IVisualState

			// Get Instance of VisualState base class

			// Serialize all property data

			// Send as response over socket.

		}

		private async Task RunServer(string host, int port)
		{
			try
			{
				TcpListener listener = new TcpListener(IPAddress.Parse(host), port);

				listener.Start();

				var client = await listener.AcceptTcpClientAsync();

				using (var stream = client.GetStream())
					while (client.Connected)
					{
						if (stream.DataAvailable)
						{
							var b = new byte[stream.Length];
							await stream.ReadAsync(b, 0, Convert.ToInt32(stream.Length));

							ProcessData(System.Text.Encoding.UTF8.GetString(b));
						}
						await Task.Delay(200);
					}

				listener.Stop();
			}
			catch (Exception ex)
			{

			}
		}

		private void ProcessData(string data)
		{
			if (string.IsNullOrEmpty(data))
				return;


		}

		private IList<IPropertyState> GetCurrentVisualState()
		{
			return null;
		}


		// Server (mobile), Desktop Client (WPF)

		// Inspector load from app.xaml.cs

		// Hook onto navigation service on page load and inject the visual state reference

		// Get base list of properties and value changes

		// Use reflection to change them (plus disable replay mapping)

		// Trigger replay from socket server

	}
}
