namespace Exrin.Abstraction
{
    public interface INavigationView: IViewContainer
    {        
        IStack MainStack { get; set; }
    }
}
