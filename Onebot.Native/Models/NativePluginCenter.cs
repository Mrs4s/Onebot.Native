using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Onebot.Native.Core;
using Onebot.Native.Core.CQ;
using Onebot.Native.Core.Extensions;
using Onebot.Native.Core.Models;
using Onebot.Native.Models.Adapters;
using Onebot.Native.Models.Onebot;

namespace Onebot.Native.Models
{
    public class NativePluginCenter
    {
        public static NativePluginCenter Center { get; } = new NativePluginCenter();

        private NativePluginCenter() { }

        /// <summary>
        /// 加载CQ的插件
        /// </summary>
        /// <param name="adapter"></param>
        public void LoadCQPlugins(IOnebotAdapter adapter)
        {
            CQAppManager.Api = new CQApiAdapter(adapter);
            CQAppManager.Manager.LoadApps("apps", "data");
            CQAppManager.Manager.EnableApps();


            // --- 转发事件 ---
            adapter.OnGroupMessage += token =>
            {
                CQAppManager.Manager.InvokeGroupMsgEvent(1,
                    token["message_id"].ToObject<int>(), token["group_id"].ToObject<long>(),
                    token.SelectToken("sender.user_id").ToObject<long>(), "", OnebotTools.ParseMessageToString(token["message"]), 0);
            };
            adapter.OnPrivateMessage += token =>
            {
                var subType = token["sub_type"]?.ToString() == "friend" ? 11 : 2;
                CQAppManager.Manager.InvokePrivateMsgEvent(subType,
                    token["message_id"].ToObject<int>(), token["user_id"].ToObject<long>(), OnebotTools.ParseMessageToString(token["message"]),
                    0);
            };
            adapter.OnGroupUpload += token =>
            {
                var file = token["file"]?.ToObject<GroupFileInfo>();
                if (file == null) return;
                using(var ms = new MemoryStream())
                using (var packet = new BytesPacket(ms))
                {
                    var encode = packet
                        .WriteString(file.Id)
                        .WriteString(file.Name)
                        .WriteInt64(file.Size)
                        .WriteInt32(file.Busid)
                        .ToBase64();
                    CQAppManager.Manager.InvokeGroupUploadEvent(token["group_id"].ToObject<long>(),
                        token["user_id"].ToObject<long>(), token["time"].ToObject<long>(), encode);
                }
            };
        }

        /// <summary>
        /// 禁用CQ的插件
        /// </summary>
        public void DisableCQPlugins() => CQAppManager.Manager.DisableApps();

        /// <summary>
        /// 初始化CQ插件的菜单
        /// </summary>
        public void InitCQMenu(MenuItem item)
        {
            foreach (var app in CQAppManager.Manager.Apps.Where(v=>v.Enabled))
            {
                var subItem = new MenuItem(app.AppInfo.Name);
                foreach (var appMenu in app.AppInfo.Menus)
                    subItem.MenuItems.Add(new MenuItem(appMenu.Name).Also(v =>
                        v.Click += (sender, args) => Task.Factory.StartNew(() => app.InvokeMenu(appMenu.Name))));
                item.MenuItems.Add(subItem);
            }
        }
    }
}
