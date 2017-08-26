namespace ExrinSampleMobileApp.Container
{
	using Exrin.Abstraction;
	using Exrin.Framework;
	using ExrinSampleMobileApp.Proxy;
	using Framework.Locator;
	using Logic.Stack;
	using Xamarin.Forms;

	public class MainContainer : ViewContainer, IMasterDetailContainer
    {
		private MasterDetailPage page;
		public MainContainer(TabbedViewContainer mainStack, MenuStack menuStack)
			: base(Containers.Main.ToString())
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

		private IHolder _detailStack;
		public IHolder DetailStack { get { return _detailStack; } set { _detailStack = value; if (_detailStack is ITabbedContainer container) ((ViewContainer)container).ParentContainer = this; } }

		private IHolder _masterStack;
		public IHolder MasterStack { get { return _masterStack; } set { _masterStack = value; if (_masterStack is ITabbedContainer container) ((ViewContainer)container).ParentContainer = this; } }

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
