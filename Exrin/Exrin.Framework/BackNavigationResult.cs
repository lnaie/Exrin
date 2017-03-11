namespace Exrin.Framework
{
    public class BackNavigationResult: Result
    {
        public BackNavigationResult()
        {
            base.ResultAction = Abstraction.ResultType.Navigation;
            base.Arguments = new BackNavigationArgs();
        }

        public BackNavigationResult(object backParameter)
        {
            base.ResultAction = Abstraction.ResultType.Navigation;
            base.Arguments = new BackNavigationArgs() { Parameter = backParameter };
        }
    }
}
