using System;
using System.Collections.Generic;

namespace Exrin.Inspector
{
    public class PropertyState
    {

        public string Name { get; set; }

        public Dictionary<DateTime, object> ValueChanges { get; set; }

    }
}
