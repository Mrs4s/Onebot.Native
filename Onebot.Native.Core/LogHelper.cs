using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onebot.Native.Core.Extensions;

namespace Onebot.Native.Core
{
    public static class LogHelper
    {
        public static string LogDir => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

        public static string LogFile =>
            Path.Combine(LogDir, $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}.log");

        private static readonly object Lock = new object();

        public static event Action<string, string, object, LogLevel> OnLogWrite;

        public static void Write(string from, string type, object content, LogLevel level = LogLevel.Info)
        {
            lock (Lock)
            {
                Init();
                using (var sw = new StreamWriter(LogFile, true))
                {
                    var str = $"[{DateTime.Now:T}] [{type}] [INFO]: {content}";
                    //Console.WriteLine(str);
                    sw.WriteLine(str);
                }
            }
            OnLogWrite?.Invoke(from, type, content, level);
        }

        public static void Error(string from, string type, object content, Exception ex = null)
        {
            lock (Lock)
            {
                Init();
                using (var sw = new StreamWriter(LogFile, true))
                {
                    var str = $"[{DateTime.Now:T}] [{type}] [ERROR]: {content}" +
                              (ex == null ? string.Empty : "\r\n" + ex);
                    //Console.WriteLine(str);
                    sw.WriteLine(str);
                }
            }
        }

        private static void Init()
        {
            if (!Directory.Exists(LogDir))
                Directory.CreateDirectory(LogDir);
        }
    }
    public enum LogLevel
    {
        [Alias(KnownColor.Gray)]
        Debug = 0,
        [Alias(KnownColor.Black)]
        Info = 10,
        [Alias(KnownColor.Magenta)]
        InfoSuccess = 11,
        [Alias(KnownColor.Blue)]
        InfoReceive = 12,
        [Alias(KnownColor.Green)]
        InfoSend = 13,
        [Alias(KnownColor.DarkOrange)]
        Warning = 20,
        [Alias(KnownColor.Red)]
        Error = 30,
        [Alias(KnownColor.DarkRed)]
        Fatal = 40
    }
}
