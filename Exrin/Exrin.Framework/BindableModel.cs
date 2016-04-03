using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class BindableModel : INotifyPropertyChanged, IDisposable
    {

        private IDictionary<string, object> _propertyValues = null;

        public BindableModel()
        {
            _propertyValues = new Dictionary<string, object>();
        }

        //TODO: When C#7 is released, replace with Sideways Loading for INPC

        public T Get<T>([CallerMemberName] string propertyName = "")
        {
            if (_propertyValues.ContainsKey(propertyName))
                return (T)_propertyValues[propertyName];
            else
                return default(T);
        }

        public void Set(object value, [CallerMemberName] string propertyName = "")
        {
            if (_propertyValues.ContainsKey(propertyName))
                _propertyValues[propertyName] = value;
            else
                _propertyValues.Add(propertyName, value);
            
            OnPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        public virtual void Dispose()
        {
            PropertyChanged = null;
        }
    }
}
