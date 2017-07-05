namespace Exrin.Framework
{
	using Abstraction;

	public class SingleViewContainer : ViewContainer, ISingleContainer
	{
		public SingleViewContainer(string identifier, IStack stack) : base(identifier, stack.Proxy.NativeView)
		{
			Stack = stack;
		}

	

		public IStack Stack { get; set; }
	}
}
