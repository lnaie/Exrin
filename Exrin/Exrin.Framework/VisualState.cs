using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;

namespace Exrin.Framework
{
    public class VisualState : BindableModel, IVisualState
    {

        protected IBaseModel Model { get; set; }

        public VisualState(IBaseModel model)
        {
			if (model == null)
				throw new ArgumentNullException($"{nameof(VisualState)} can not have a null {nameof(IBaseModel)}");

            Model = model;
            HookEvents();
        }

		public virtual void Init() { }

		protected virtual void OnModelStatePropertyChanged(string propertyName)
		{
			try
			{
				var modelState = Model.GetType().GetRuntimeProperty("ModelState").GetValue(Model);

				var value = modelState.GetType().GetRuntimeProperty(propertyName).GetValue(modelState);

				this.GetType().GetRuntimeProperty(propertyName)?.SetValue(this, value);
			}
			catch
			{
				Debug.WriteLine($"Binding of the {nameof(ModelState)} to {nameof(VisualState)} for property {propertyName} failed.");
			}
			
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnModelStatePropertyChanged(e.PropertyName);
        }

        protected void HookEvents()
        {
            Model.ModelState.PropertyChanged += OnPropertyChanged;
        }
        public override void Disposing()
        {
            Model.ModelState.PropertyChanged -= OnPropertyChanged;
        }
        ~VisualState()
        {
            Dispose(false);
        }

    }
}
