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
    }
}
