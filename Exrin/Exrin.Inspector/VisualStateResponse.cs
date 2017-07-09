using System.Collections.Generic;

namespace Exrin.Inspector
{
	public class VisualStateResponse: Response
    {
		public List<PropertyState> Properties { get; set; }
	}
}
