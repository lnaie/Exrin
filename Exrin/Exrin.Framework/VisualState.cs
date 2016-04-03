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

        protected virtual IBaseModel Model { get; set; }

        public VisualState()
        {
           
        }
        
        protected virtual void OnModelPropertyChanged(string propertyName)
        { }
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnModelPropertyChanged(e.PropertyName);
        }

        protected void HookEvents()
        {
            Model.PropertyChanged += OnPropertyChanged;
        }
        public override void Disposing()
        {
            Model.PropertyChanged -= OnPropertyChanged;
        }
        ~VisualState()
        {
            Dispose(false);
        }

    }
}
