namespace Exrin.Framework
{
	using System;
	using System.Collections.Generic;
	using Abstraction;

	public class ViewContainer : IViewContainer
	{
		public ViewContainer(string identifier)
		{
			Identifier = identifier;
		}

		public ViewContainer(string identifier, object view)
		{
			Identifier = identifier;
			NativeView = view;
		}
		public string Identifier { get; private set; }

		public object NativeView { get; set; }

		public IDictionary<object, ContainerType> RegionMapping { get; } = new Dictionary<object, ContainerType>();
	}
}
