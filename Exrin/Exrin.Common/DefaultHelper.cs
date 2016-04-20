using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Common
{
    public static class DefaultHelper
    {
        
        public static async Task<T> GetOrDefaultAsync<T>(this Func<Task<T>> function, T defaultValue)
        {
            try
            {
                return await function();
            }
            catch
            {
                //TODO: some debug output here would be worthwhile
                return defaultValue;
            }
        }

        public static T GetOrDefault<T>(this Func<T> function, T defaultValue)
        {
            try
            {
                return function();
            }
            catch
            {
                return defaultValue;
            }
        }

    }
}
