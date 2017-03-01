using Exrin.Abstraction;
using ExrinSampleMobileApp.Proxy;
using System;
using System.Reflection;
using Xamarin.Forms;

namespace ExrinSampleMobileApp
{
    public class Bootstrapper : Exrin.Framework.Bootstrapper
    {
        private static Bootstrapper _instance = null;

        public Bootstrapper(IInjectionProxy injection, Action<object> setRoot)
            : base(injection, setRoot) { }

        public static Bootstrapper GetInstance()
        {
            if (_instance == null)
                _instance = new Bootstrapper(new Injection(),
                                            (view) =>
                                            {
                                                Application.Current.MainPage = view as Page;
                                            });

            _instance.RegisterAssembly(AssemblyAction.Bootstrapper, new Framework.AssemblyRegister().GetType().GetTypeInfo().Assembly.GetName());
            _instance.RegisterAssembly(AssemblyAction.Bootstrapper, new Logic.AssemblyRegister().GetType().GetTypeInfo().Assembly.GetName());
            _instance.RegisterAssembly(AssemblyAction.Bootstrapper, new View.AssemblyRegister().GetType().GetTypeInfo().Assembly.GetName());

            return _instance;
        }
    }
}
