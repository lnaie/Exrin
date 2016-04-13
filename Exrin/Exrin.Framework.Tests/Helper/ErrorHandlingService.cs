using Exrin.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Framework.Tests.Helper
{
    public class ErrorHandlingService : IErrorHandlingService
    {
        public Task HandleError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
