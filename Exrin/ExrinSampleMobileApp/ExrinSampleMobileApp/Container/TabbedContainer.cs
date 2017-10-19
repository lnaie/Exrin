using Exrin.Abstraction;
using ExrinSampleMobileApp.Framework.Locator;
using ExrinSampleMobileApp.Logic.Stack;
using ExrinSampleMobileApp.Proxy;
using System.Collections.Generic;

namespace ExrinSampleMobileApp.Container
{
	public class TabbedViewContainer : Exrin.Framework.ViewContainer, ITabbedContainer
	{
		private TabbedProxy _proxy;
		public TabbedViewContainer(MainStack mainStack, SecondStack secondStack)
			: base(Containers.Tabbed.ToString(), null)
		{
			Children = new List<IStack>() { mainStack, secondStack };
			var tabbedPage = new Xamarin.Forms.TabbedPage();
			_proxy = new TabbedProxy(tabbedPage);
			NativeView = _proxy.View;
			RegionMapping.Add("Master", ContainerType.Master);
			RegionMapping.Add("Detail", ContainerType.Detail);

			foreach (var child in Children)
				_proxy.Children.Add(child.Proxy.NativeView);
		}

		public IList<IStack> Children { get; set; }

		public void SetCurrentPage(IStack stack)
		{
			var index = Children.IndexOf(stack);
			_proxy.CurrentPage = Children[index];
		}
	}
}
