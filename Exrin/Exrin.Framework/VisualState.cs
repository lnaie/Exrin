using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class VisualState : BindableModel, IVisualState
    {

        protected IBaseModel Model { get; set; }

        public VisualState(IBaseModel model)
        {
            Model = model;
            HookEvents();
        }
        
        protected virtual void OnModelStatePropertyChanged(string propertyName)
        { }
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
