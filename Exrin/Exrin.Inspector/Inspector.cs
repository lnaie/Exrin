using Exrin.Abstraction;
using Exrin.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
							byte[] resp = new byte[2048];
							var memStream = new MemoryStream();
							int bytes = stream.Read(resp, 0, resp.Length);
							while (bytes > 0)
							{
								memStream.Write(resp, 0, bytes);
								bytes = 0;
								if (stream.DataAvailable)
									bytes = stream.Read(resp, 0, resp.Length);
							}
							
							ProcessData(System.Text.Encoding.UTF8.GetString(memStream.ToArray()), stream);
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
			else
			{
				foreach (var item in split)
					ProcessCommand(item, stream);

				_data = string.Empty;
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
			catch   (Exception ex)
			{
				// Unknown error, ignore for the moment, and continue on
				var msg = ex.Message;
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
			var visualState = GetCurrentVisualState() as BindableModel;
		
			var propertyList = new List<PropertyState>();
			
			foreach (var property in visualState.GetStateHistory())
			{
				var propertyName = property.Value.Key;
				if (!propertyList.Any(x => x.Name == propertyName))
					propertyList.Add(new PropertyState() { Name = propertyName, ValueChanges = new Dictionary<DateTime, object>() });

				propertyList.Single(x => x.Name == propertyName).ValueChanges.Add(property.Key, property.Value.Value);

			}

			return new VisualStateResponse() { Type = CommandType.VisualState, Properties = propertyList };
		}
		
		private IVisualState GetCurrentVisualState()
		{
			// ISSUE - requires concrete class
			var stackIdentifier = (_navigationService as NavigationService).GetType().GetRuntimeFields().First(x=>x.Name == "_currentStack").GetValue(_navigationService) as object;
			var stacks = (_navigationService as NavigationService).GetType().GetRuntimeFields().First(x => x.Name == "_stacks").GetValue(_navigationService) as IDictionary<object, IStack>;
			var stack = stacks[stackIdentifier] as IStack;

			var currentPage = stack.Proxy.NativeView.GetType().GetRuntimeProperty("CurrentPage").GetValue(stack.Proxy.NativeView);

			var viewModel = currentPage.GetType().GetRuntimeProperty("BindingContext").GetValue(currentPage) as IViewModel;

			return viewModel.VisualState;
		}

	}
}
