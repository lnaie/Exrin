using System;
using System.Collections.Generic;

namespace Exrin.Inspector
{
    public interface IPropertyState
    {

        string Name { get; set; }

        IDictionary<DateTime, object> ValueChanges { get; set; }

    }
}
