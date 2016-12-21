namespace Exrin.Framework
{
    using Abstraction;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reflection;

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

        private bool _isBusy = false;
        public bool IsBusy { get { return _isBusy; } set { _isBusy = value; OnPropertyChanged(); } }

        /// <summary>
        /// Will copy the value from the Model State into the Visual State when the Model property changes
        /// </summary>
        /// <param name="propertyName"></param>
		protected virtual void OnModelStatePropertyChanged(string propertyName)
		{
			try
			{
				var modelState = Model.GetType().GetRuntimeProperty("ModelState").GetValue(Model);

				var value = modelState.GetType().GetRuntimeProperty(propertyName).GetValue(modelState);

                var existingValue = this.GetType().GetRuntimeProperty(propertyName)?.GetValue(this);

                if (existingValue != value)
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
