using Exrin.Abstraction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;

namespace Exrin.Inspector
{
	// Exrin Experimental

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

							ProcessData(System.Text.Encoding.UTF8.GetString(b), stream);
						}
						await Task.Delay(200);
					}

				listener.Stop();

				
			}
			catch
			{
				await Task.Delay(20000); // Wait, to give the platform some time.
			}

			// Start Listening Again, on new thread, don't wait
			await Task.Factory.StartNew(async () => await RunServer(host, port));
		}


		private string _data = string.Empty;
		public const char EOT = '\u0004';
		private void ProcessData(string data, NetworkStream stream)
		{
			if (string.IsNullOrEmpty(data))
				return;

			_data += data;

			string[] split = null;

			bool endOf = _data.EndsWith(EOT.ToString());

			if (_data.IndexOf(EOT) > -1)
				split = _data.Split(new[] { EOT }, StringSplitOptions.RemoveEmptyEntries);

			if (split == null)
				return;

			if (split.Length == 1 && !endOf)
				return;

			if (!endOf)
			{
				_data = split[split.Length - 1]; // Last unfinished command back to be processed
				var list = split.ToList();
				list.RemoveAt(split.Length - 1);

				foreach (var item in list)
					ProcessCommand(item, stream);
			}
		}

		private void ProcessCommand(string item, NetworkStream stream)
		{
			try
			{
				// 1st Command - Get Current Visual State
				var command = JsonConvert.DeserializeObject<Command>(item);

				switch (command.Type)
				{
					case CommandType.VisualState:
						SendResponse(GetVisualStateResponse(), stream);
						break;
				}


			}
			catch (JsonException ex)
			{
				// Conversion Exception
			}
			catch   
			{
				// Unknown error, ignore for the moment, and continue on
			}
		}

		private void SendResponse(Response response, NetworkStream stream)
		{
			var data = JsonConvert.SerializeObject(response) + EOT;
			var b = System.Text.Encoding.UTF8.GetBytes(data);
			stream.Write(b, 0, b.Length);
		}

		private VisualStateResponse GetVisualStateResponse()
		{
			// Get the current View
			var visualState = GetCurrentVisualState();

			var propertyList = new List<PropertyState>();

			foreach (var propertyInfo in visualState.GetType().GetRuntimeProperties())
			{
				var property = propertyInfo.GetValue(visualState);
				propertyList.Add(new PropertyState() { Name = propertyInfo.Name });
			}

			return new VisualStateResponse() { Type = CommandType.VisualState };
		}


		private IVisualState GetCurrentVisualState()
		{
			var stack = _navigationService.GetType().GetRuntimeField("_currentStack").GetValue(_navigationService) as IStack;

			var viewModel = stack.Proxy.NativeView.GetType().GetRuntimeProperty("BindingContext").GetValue(stack.Proxy.NativeView) as IViewModel;

			return viewModel.VisualState;
		}

	}
}
