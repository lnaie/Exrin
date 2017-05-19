using Exrin.Abstraction;
using Exrin.Framework;
using ExrinSampleMobileApp.Framework.Abstraction.ModelState;

namespace ExrinSampleMobileApp.Logic.ModelState
{
    public class MainModelState : Exrin.Framework.ModelState, IMainModelState
    {
		[Binding(BindingType.TwoWay)]
		public object MyProperty { get { return Get<object>(); } set { Set(value); } }
	}
}
