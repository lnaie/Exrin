using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class PropertyValueChangedEventArgs: PropertyChangedEventArgs
    {
        public object OldValue { get; set; }
        public object NewValue { get; set; }

        public PropertyValueChangedEventArgs(string propertyName, object oldValue, object newValue): base (propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

    }
}
