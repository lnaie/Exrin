namespace ExrinSampleMobileApp.Container
{
    using Exrin.Abstraction;
    using Framework.Locator;
    using Logic.Stack;

    public class MainContainer : Exrin.Framework.ViewContainer, ISingleContainer
    {

        public MainContainer(MainStack stack)
            : base(Containers.Main.ToString(), stack.Proxy.NativeView)
        {
            Stack = stack;
        }

        public IStack Stack { get; set; }

    }
}
