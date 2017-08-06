using System;
using System.Reflection;
using Exrin.Abstraction;

namespace Exrin.Framework
{
	public class Isolate : IIsolate
	{
		private AssemblyName _name;
		public AssemblyName Name
		{
			get
			{
				return _name ?? (_name = GetType().GetTypeInfo().Assembly.GetName());
			}
		}
	}
}
