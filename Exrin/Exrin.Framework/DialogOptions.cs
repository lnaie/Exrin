using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class DialogOptions : IDialogOptions
    {
        public string Message { get; set; }

		public bool Result { get; set; }

		public string Title { get; set; }
    }
}
