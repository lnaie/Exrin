using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Common
{
	public static class StringHelper
	{

		public static string GetFilename(this string value, bool stripExtension = false)
		{
			if (String.IsNullOrEmpty(value))
				return string.Empty;

			var marker = "";
			if (value.Contains("\\"))
				marker = "\\";
			else if (value.Contains("/"))
				marker = "/";

			if (string.IsNullOrEmpty(marker))
				return value;

			var filename = value.Substring(value.LastIndexOf(marker) + marker.Length);

			if (!stripExtension)
				return filename;

			if (filename.Contains("."))
				return filename.Substring(0, filename.LastIndexOf("."));

			return filename;
		}

	}
}
