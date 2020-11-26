using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Onebot.Native.Core.Extensions
{
    public static class StringExtensions
    {
        public static string RandomString
        {
            get
            {
                var b = new byte[4];
                new RNGCryptoServiceProvider().GetBytes(b);
                var r = new Random(BitConverter.ToInt32(b, 0));
                var ret = string.Empty;
                var str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                for (var i = 0; i < 48; i++)
                {
                    ret += str.Substring(r.Next(0, str.Length - 1), 1);
                }
                return ret;
            }
        }
    }
}
