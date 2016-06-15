using System;
using System.Diagnostics;
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
            catch (Exception ex)
            {
				Debug.WriteLine(ex.Message);
                return defaultValue;
            }
        }

        public static T GetOrDefault<T>(this Func<T> function, T defaultValue)
        {
            try
            {
                return function();
            }
            catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return defaultValue;
            }
        }

    }
}
