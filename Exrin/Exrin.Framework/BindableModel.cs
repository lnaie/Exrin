namespace Exrin.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class BindableModel : INotifyPropertyChanged, IDisposable
    {

        private IDictionary<string, object> _propertyValues = null;

        public BindableModel()
        {
            _propertyValues = new Dictionary<string, object>();
        }

        //TODO: When C#7 is released, possible replace with Sideways Loading for INPC

        public T Get<T>([CallerMemberName] string propertyName = "")
        {
            if (_propertyValues.ContainsKey(propertyName))
                return (T)_propertyValues[propertyName];
            else
                return default(T);
        }

        public void Set(object value, [CallerMemberName] string propertyName = "")
        {

            object oldValue = null; // Only works with value types
            if (_propertyValues.ContainsKey(propertyName))
            {
                oldValue = _propertyValues[propertyName];
                if (oldValue == value) // TODO: Should I block any non-changing property changes?
                    return;

                _propertyValues[propertyName] = value;
            }
            else
                _propertyValues.Add(propertyName, value);



            OnPropertyChanged(oldValue, value, propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void OnPropertyChanged(object oldValue, object newValue, [CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyValueChangedEventArgs(name, oldValue, newValue));
        }

        public virtual void Disposing()
        {

        }

        private bool _disposed = false;
        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Disposing();
                }
                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }
    }
}
