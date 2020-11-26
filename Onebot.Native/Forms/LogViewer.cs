using Onebot.Native.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Onebot.Native.Core.Extensions;

namespace Onebot.Native.Forms
{
    public partial class LogViewer : Form
    {
        public static LogViewer Form { get; } = new LogViewer();

        private LogViewer()
        {
            InitializeComponent();
            LogHelper.OnLogWrite += OnLogWrite;
            Show();
            Hide();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void LogViewer_Load(object sender, EventArgs e)
        {
            
        }

        private void OnLogWrite(string from, string type, object content, LogLevel level)
        {
            LogListView.Invoke(new MethodInvoker(() =>
            {
                LogListView.Items.Add(new ListViewItem(DateTime.Now.ToString("HH:mm:ss")).Also(v =>
                {
                    v.SubItems.Add(from);
                    v.SubItems.Add(type);
                    v.SubItems.Add(content.ToString());
                    v.SubItems.Add("-");
                    v.ForeColor = Color.FromKnownColor(level.GetAlias<KnownColor>());
                }));
                if (!RealTimeCheckbox.Checked) return;
                LogListView.EnsureVisible(LogListView.Items.Count - 1);
                LogListView.Items[LogListView.Items.Count - 1].Selected = true;
            }));
        }

        private void LogViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
