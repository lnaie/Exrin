namespace Exrin.Framework
{
    using System.ComponentModel;

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
