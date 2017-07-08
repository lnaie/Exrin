using System;

namespace Exrin.Abstraction
{
	public interface IView
    {
        object BindingContext { get; set; } 

        event EventHandler Appearing;      
        event EventHandler Disappearing;

        Func<bool> OnBackButtonPressed { get; set; }

    }
}
