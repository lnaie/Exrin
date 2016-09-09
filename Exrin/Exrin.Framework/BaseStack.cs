namespace Exrin.Framework
{
    using Abstraction;
    using System;
    using System.Threading.Tasks;

    public class BaseStack: IStack
    {
        protected readonly INavigationService _navigationService;

        public object StackIdentifier { get; set; }

        public BaseStack(INavigationService navigationService, INavigationContainer navigationContainer, IMasterDetailView masterView, object stackIdentifier)
        {
            MasterView = masterView;            
            _navigationService = navigationService;
            SetContainer(navigationContainer);
            StackIdentifier = stackIdentifier;
        }

        public BaseStack(INavigationService navigationService, INavigationContainer navigationContainer, object stackIdentifier)
        {
            _navigationService = navigationService;
            SetContainer(navigationContainer);
            StackIdentifier = stackIdentifier;
        }

        public void Init()
        {
            Map();
        }

        public bool ShowNavigationBar { get; set; } = true;

        public INavigationContainer Container { get; private set; }
        
        public StackStatus Status { get; set; } = StackStatus.Stopped;

        public async Task StartNavigation(object args = null, bool loadStartKey = true)
        {
            // Assign Master but hold static
            if (MasterView != null && MasterView.MasterView == null)
                MasterView.MasterView = await _navigationService.BuildView(MasterStartKey, args); // CHECK: see if it can be built elsewhere
            
            if (loadStartKey)
                await _navigationService.Navigate(NavigationStartKey, args);
           
            Status = StackStatus.Started;
        }

        /// <summary>
        /// Will register appropriate Services for Dependency Injection.
        /// </summary>
        protected void SetContainer(INavigationContainer container)
        {
            Container = container;

            if (MasterView != null) // Set Detail Page as Container
                MasterView.DetailView = Container.View;
        }

        protected virtual void Map() { }

		protected void NavigationMap(string key, Type view, Type viewModel) {
			_navigationService.Map(StackIdentifier, key, view, viewModel);
		}

		protected virtual string NavigationStartKey { get; }

        protected virtual string MasterStartKey { get; }

        public IMasterDetailView MasterView { get; private set; }
    }
}
