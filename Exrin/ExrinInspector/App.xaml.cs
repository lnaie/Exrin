using Exrin.Inspector;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExrinInspector
{


	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		// Forgive me, for I have sinned
		// This project is experimental


		public static TcpClient Client;
		public delegate void ResponseReceivedHandler(Response response);
		public event ResponseReceivedHandler ResponseReceived;

		public async Task MonitorStream()
		{
			using (var stream = Client.GetStream())
				while (Client.Connected)
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

						ProcessData(Encoding.UTF8.GetString(memStream.ToArray()));
					}
					await Task.Delay(200);
				}
		}

		private string _data = string.Empty;
		public const char EOT = '\u0004';
		private void ProcessData(string data)
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
					ProcessResponse(item);
			}
			else
			{
				foreach (var item in split)
					ProcessResponse(item);

				_data = string.Empty;
			}

		}

		private void ProcessResponse(string item)
		{
			try
			{
				var response = JsonConvert.DeserializeObject<VisualStateResponse>(item);

				ResponseReceived?.Invoke(response);
			}
			catch { }
		}

		public void RequestVisualState()
		{
			var command = new Command() { Type = CommandType.VisualState };
			var data = JsonConvert.SerializeObject(command) + Inspector.EOT;
			WriteString(Client.GetStream(), data);
		}

		private void WriteString(NetworkStream stream, string text)
		{
			var byteArray = Encoding.UTF8.GetBytes(text);

			stream.Write(byteArray, 0, byteArray.Length);
		}
	}
}
