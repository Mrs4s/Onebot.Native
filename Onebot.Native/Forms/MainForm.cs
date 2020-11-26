using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Onebot.Native.Core;
using Onebot.Native.Core.Extensions;
using Onebot.Native.Models;
using Onebot.Native.Models.Onebot;
using Onebot.Native.Properties;

namespace Onebot.Native.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void StartupButton_Click(object sender, EventArgs e)
        {
            var address = AddressTextbox.Text;
            if (!int.TryParse(PortTextbox.Text, out var port) || port < 1 || port >= ushort.MaxValue)
            {
                MessageBox.Show("非法端口", "错误");
                return;
            }
            LogViewer.Form.Show();
            var server = new OnebotWebsocketServer($"ws://{address}:{port}", AccessTokenTextbox.Text);
            server.Run();
            StartupButton.Enabled = false;
            StartupButton.Text = "等待连接";
            server.OnConnected += s =>
            {
                Invoke(new MethodInvoker(Hide));
                NativePluginCenter.Center.LoadCQPlugins(server);
                NativePluginCenter.Center.InitCQMenu(Notify.ContextMenu.MenuItems[0]);
            };
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Icon = Resources.onebot;
            Notify.Icon = Resources.onebot;
            Notify.ContextMenu = new ContextMenu().Also(v =>
            {
                v.MenuItems.Add(new MenuItem {Text = "应用", Name = "Plugin",MenuItems = { }});
                v.MenuItems.Add(
                    new MenuItem {Text = "日志", Tag = "Log"}.Also(m => m.Click += (o, args) => LogViewer.Form.Show()));
                v.MenuItems.Add("-");
                v.MenuItems.Add(new MenuItem {Text = "重载应用", Tag = "ReLoad"}.Also(m =>
                    m.Click += (o, args) => MessageBox.Show("暂不支持, 请重启框架( ", "咕咕")));
                v.MenuItems.Add(new MenuItem {Text = "退出", Tag = "Quit"}.Also(m => m.Click += (o, args) =>
                {
                    var reset = new ManualResetEvent(false);
                    Task.Factory.StartNew(() =>
                    {
                        NativePluginCenter.Center.DisableCQPlugins(); 
                        reset.Set();
                    });
                    reset.WaitOne(TimeSpan.FromSeconds(3)); // 超时
                    //Environment.Exit(0); // 某些native插件会导致无法退出, 草
                    Process.GetCurrentProcess().Kill();
                }));
            });
            if (Directory.Exists("bin"))
            {
                foreach (var file in Directory.GetFiles("bin").Where(v => Path.GetExtension(v) == ".dll"))
                {
                    if (Win32.LoadLibrary(file))
                        LogHelper.Write("Onebot.Native", "预加载", $"库 {Path.GetFileName(file)} 成功加载.",
                            LogLevel.InfoSuccess);
                    else
                        LogHelper.Write("Onebot.Native", "预加载", $"库 {Path.GetFileName(file)} 加载失败.",
                            LogLevel.Warning);
                }
            }
        }
    }
}
