namespace Exrin.Framework
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Reflection;
	using System.Runtime.CompilerServices;

	public class BindableModel : INotifyPropertyChanged, IDisposable
	{
		private IDictionary<DateTime, KeyValuePair<string, object>> _propertyValueTrack = null;
		private IDictionary<string, object> _propertyValues = null;

		public BindableModel()
		{
			_propertyValues = new Dictionary<string, object>();
			_propertyValueTrack = new Dictionary<DateTime, KeyValuePair<string, object>>();

			if (App.PlatformOptions.StateTracking)
				LoadDefaults();
		}

		private void LoadDefaults()
		{
			foreach (var propertyInfo in this.GetType().GetRuntimeProperties())
			{
				try
				{
					var property = propertyInfo.GetValue(this);
					Set(property, propertyInfo.Name);
				}
				catch { }
			}
		}
		
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
				if (oldValue == value)
					return;

				_propertyValues[propertyName] = value;
			}
			else
				_propertyValues.Add(propertyName, value);

			if (App.PlatformOptions.StateTracking)
				_propertyValueTrack.Add(DateTime.Now, new KeyValuePair<string, object>(propertyName, value));

			if (oldValue == null && value == null)
				return;

			if ((oldValue == null && value != null)
				|| (oldValue != null && value == null))
			{
				OnPropertyChanged(oldValue, value, propertyName);
				return;
			}

			if (!oldValue.Equals(value)) // Only when property changed
				OnPropertyChanged(oldValue, value, propertyName);
		}

		public IDictionary<DateTime, KeyValuePair<string, object>> GetStateHistory()
		{
			return _propertyValueTrack;
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

		public virtual void Disposing() { }

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
