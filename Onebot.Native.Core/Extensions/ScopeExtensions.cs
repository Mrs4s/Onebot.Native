using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onebot.Native.Core.Extensions
{
    public static class ScopeExtensions
    {
        public static T Also<T>(this T @this, Action<T> block)
        {
            block(@this);
            return @this;
        }
        public static TR Let<T, TR>(this T @this, Func<T, TR> block) => block(@this);

        public static async Task<TR> LetAsync<T, TR>(this Task<T> @this, Func<T, TR> block) => block(await @this);


        public static async Task<T> RunCatch<T>(this Task<T> @this, Func<Exception, T> block = null) 
        {
            try
            {
                return await @this;
            }
            catch (Exception e)
            {
                return block !=  null ? block(e) : default;
            }
        }

        public static async Task<bool> RunSuccessful<T>(this Task<T> @this)
        {
            try
            {
                await @this;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<T> RunCatchDefault<T>(this Task<T> @this, Action<Exception> block = null)
        {
            try
            {
                return await @this;
            }
            catch (Exception ex)
            {
                block?.Invoke(ex);
                return default;
            }
        }
    }
}
