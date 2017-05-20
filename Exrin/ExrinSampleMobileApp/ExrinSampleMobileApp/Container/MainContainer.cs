namespace ExrinSampleMobileApp.Container
{
	using Exrin.Abstraction;
	using ExrinSampleMobileApp.Proxy;
	using Framework.Locator;
	using Logic.Stack;
	using Xamarin.Forms;

	public class MainContainer : Exrin.Framework.ViewContainer, IMasterDetailContainer
    {
		private MasterDetailPage page;
		public MainContainer(MainStack mainStack, MenuStack menuStack)
			: base(Containers.Main.ToString(), mainStack.Proxy.NativeView)
		{
			page = new MasterDetailPage();
			var mdp = new MasterDetailProxy(page);
			NativeView = mdp.View;
			Proxy = mdp;
			DetailStack = mainStack;
			MasterStack = menuStack;
			RegionMapping.Add(Regions.Menu, ContainerType.Master);
			RegionMapping.Add(Regions.Main, ContainerType.Detail);
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
