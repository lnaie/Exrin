using Exrin.Abstraction;
using ExrinSampleMobileApp.Framework.Locator;
using ExrinSampleMobileApp.Logic.Stack;
using ExrinSampleMobileApp.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExrinSampleMobileApp.Container
{
	public class TabbedViewContainer : Exrin.Framework.ViewContainer, ITabbedContainer
	{

		public TabbedViewContainer(MainStack mainStack, SecondStack secondStack)
			: base(Containers.Tabbed.ToString(), null)
		{
			Children = new List<IStack>() { mainStack, secondStack };
			var tabbedPage = new Xamarin.Forms.TabbedPage();
			var tabbed = new TabbedProxy(tabbedPage);
			NativeView = tabbed.View;

			foreach (var child in Children)
			{
				tabbed.Children.Add(child.Proxy.NativeView);
			}
		}


		public IList<IStack> Children { get; set; }

	}
}
