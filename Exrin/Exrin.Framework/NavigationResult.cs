namespace Exrin.Framework
{
    public class NavigationResult: Result
    {
        public NavigationResult(object stackType, object key )
        {
            base.ResultAction = Abstraction.ResultType.Navigation;
            base.Arguments = new NavigationArgs() { StackType = stackType, Key = key };
        }

        public NavigationResult(object stackType, object key, object parameter, bool newInstance)
        {
            base.ResultAction = Abstraction.ResultType.Navigation;
            base.Arguments = new NavigationArgs() { StackType = stackType, Key = key, Parameter = parameter, NewInstance = newInstance };
        }

        public NavigationResult(object stackType, object key, object parameter, bool newInstance, bool popSource)
        {
            base.ResultAction = Abstraction.ResultType.Navigation;
            base.Arguments = new NavigationArgs() { StackType = stackType, Key = key, Parameter = parameter, NewInstance = newInstance, PopSource = popSource };
        }

		public NavigationResult(object containerId, object regionId, object stackType, object key)
		{
			base.ResultAction = Abstraction.ResultType.Navigation;
			base.Arguments = new NavigationArgs() { ContainerId = containerId, RegionId = regionId, StackType = stackType, Key = key };
		}

		public NavigationResult(object containerId, object regionId, object stackType, object key, object parameter, bool newInstance)
		{
			base.ResultAction = Abstraction.ResultType.Navigation;
			base.Arguments = new NavigationArgs() { ContainerId = containerId, RegionId = regionId, StackType = stackType, Key = key, Parameter = parameter, NewInstance = newInstance };
		}

		public NavigationResult(object containerId, object regionId, object stackType, object key, object parameter, bool newInstance, bool popSource)
		{
			base.ResultAction = Abstraction.ResultType.Navigation;
			base.Arguments = new NavigationArgs() { ContainerId = containerId, RegionId = regionId, StackType = stackType, Key = key, Parameter = parameter, NewInstance = newInstance, PopSource = popSource };
		}
	}
}
