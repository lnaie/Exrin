using Exrin.Abstraction;
using Exrin.Framework;

namespace ExrinSampleMobileApp.Logic.VisualState
{
    public class AboutVisualState : Exrin.Framework.VisualState
    {
		public AboutVisualState(IBaseModel model) : base(model) { }

		[Binding(BindingType.TwoWay)]
		public object MyProperty { get { return Get<object>(); } set { Set(value); } }
	}
}
