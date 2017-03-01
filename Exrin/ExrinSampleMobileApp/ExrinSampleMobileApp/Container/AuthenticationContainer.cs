namespace ExrinSampleMobileApp.Container
{
    using Exrin.Abstraction;
    using Framework.Locator;
    using Logic.Stack;

    public class AuthenticationContainer : Exrin.Framework.ViewContainer, ISingleContainer
    {

        public AuthenticationContainer(AuthenticationStack stack)
            : base(Containers.Authentication.ToString(), stack.Proxy.NativeView)
        {
            Stack = stack;

        }

        public IStack Stack { get; set; }

    }
}
