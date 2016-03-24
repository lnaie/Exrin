using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework
{
    public class ErrorHandlingService : IErrorHandlingService
    {
        public Task ReportError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
