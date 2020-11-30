using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Onebot.Native.Core;
using Onebot.Native.Core.Extensions;
using Onebot.Native.Forms;
using Onebot.Native.IO;
using Onebot.Native.Models;
using Onebot.Native.Models.Onebot;

namespace Onebot.Native
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*
            var s = new OnebotWebsocketServer("ws://127.0.0.1:44556");
            s.Run().GetAwaiter().GetResult();
            s.OnConnected += server =>
            {
                var a = server.DeleteMsgAsync(123).GetAwaiter().GetResult();
                Console.WriteLine(a);
            };
            Thread.Sleep(1000000);
            */
            //插件路径初始化
            Storage.InitAppStorage();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var _ = LogViewer.Form;
            Application.Run(new MainForm());
        }
    }
}
