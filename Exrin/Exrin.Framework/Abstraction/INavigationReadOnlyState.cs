namespace Exrin.Abstraction
{
    public interface INavigationReadOnlyState: INavigationState
    {
        new string ViewName { get; }
    }
}
