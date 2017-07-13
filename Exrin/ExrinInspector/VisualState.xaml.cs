using Exrin.Inspector;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;

namespace ExrinInspector
{
	/// <summary>
	/// Interaction logic for VisualState.xaml
	/// </summary>
	public partial class VisualState : Page
	{
		public VisualState()
		{
			InitializeComponent();

			(Application.Current as App).ResponseReceived += VisualState_ResponseReceived;
		}

		private void VisualState_ResponseReceived(Response response)
		{
			Dispatcher.Invoke(() =>
			{
				var vs = response as VisualStateResponse;

				// Update Tree View
				StateTreeView.Items.Clear();

				foreach (var item in vs.Properties)
				{
					var menuItem = new MenuItem()
					{
						Title = item.Name
					};

					var subItems = new ObservableCollection<MenuItem>();
					foreach (var property in item.ValueChanges)
						subItems.Add(new MenuItem() { Title = property.Key.ToString("yyyy-MM-dd HH:mm:ss") + " " + property.Value?.ToString() });

					menuItem.Items = subItems;

					StateTreeView.Items.Add(menuItem);
				}


				// Update ListView
				StateListView.Items.Clear();

				var list = new List<string>();
				foreach (var item in vs.Properties)
					foreach (var subItem in item.ValueChanges)
					 list.Add(subItem.Key.ToString("yyyy-MM-dd HH:mm:ss") + " - " + item.Name + " - " + subItem.Value?.ToString());
				
				list = list.OrderBy(x => x).ToList();

				foreach (var item in list)
					StateListView.Items.Add(new MenuItem() { Title = item });
			});


		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			// Send Command
			(Application.Current as App).RequestVisualState();
		}
	}

	public class MenuItem
	{
		public string Title { get; set; }

		public ObservableCollection<MenuItem> Items { get; set; }
	}
}
