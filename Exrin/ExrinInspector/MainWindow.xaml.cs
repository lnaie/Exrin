 using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace ExrinInspector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            TcpClient client = new TcpClient();

            client.Connect(new IPEndPoint(IPAddress.Parse(IPAddressTextBlock.Text), Convert.ToInt32(PortTextBlock.Text)));

            var stream = client.GetStream();

            WriteString(stream, "");            
        }

        private void WriteString(NetworkStream stream, string text)
        {
            var byteArray = Encoding.UTF8.GetBytes(text);

            stream.Write(byteArray, 0, byteArray.Length);
        }
    }
}
