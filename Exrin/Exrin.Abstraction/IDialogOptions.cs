using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    public interface IDialogOptions
    {        
        string Title { get; set; }
        string Message { get; set; }
		bool Result { get; set; }
    }
}
