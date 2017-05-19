namespace ExrinSampleMobileApp.Container
{
	using System;
	using Exrin.Abstraction;
	using ExrinSampleMobileApp.Proxy;
	using Framework.Locator;
	using Logic.Stack;
	using Xamarin.Forms;

	public class MainContainer : Exrin.Framework.ViewContainer, IMasterDetailContainer
    {
		private Xamarin.Forms.MasterDetailPage page;
		public MainContainer(MainStack mainStack, MenuStack menuStack)
			: base(Containers.Main.ToString(), mainStack.Proxy.NativeView)
		{
			page = new Xamarin.Forms.MasterDetailPage();
			var mdp = new MasterDetailProxy(page);
			NativeView = mdp.View;
			Proxy = mdp;
			DetailStack = mainStack;
			MasterStack = menuStack;
			ContainerMapping.Add(ContainerChildren.Menu, ContainerType.Master);
			ContainerMapping.Add(ContainerChildren.Main, ContainerType.Detail);
		}

		public IStack DetailStack { get; set; }

		public IStack MasterStack { get; set; }

		public IMasterDetailProxy Proxy { get; set; }

		public bool IsPresented
		{
			get
			{
				return page.IsPresented;
			}
			set
			{
				page.IsPresented = value;
			}
		}

		public void SetStack(ContainerType type, object newPage)
		{
			switch (type)
			{
				case ContainerType.Detail:
					page.Detail = newPage as Page;
					break;
				case ContainerType.Master:
					page.Master = newPage as Page;
					break;
			}

		}
	}
}
