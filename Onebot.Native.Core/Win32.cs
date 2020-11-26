using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Onebot.Native.Core
{
    public class Win32
    {
        /// <summary>
        /// 全局加载前置库
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool LoadLibrary(string file) => LoadLibraryA(file) != IntPtr.Zero;


        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr LoadLibraryA(string lpLibFileName);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern int FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetLastError();
    }
}
