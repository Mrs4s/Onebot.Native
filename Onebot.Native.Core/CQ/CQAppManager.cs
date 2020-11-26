using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Onebot.Native.Exports.CQP;

namespace Onebot.Native.Core.CQ
{
    public class CQAppManager : IFuncProcessor
    {
        public static CQAppManager Manager { get; } = new CQAppManager();

        public static ICQPApi Api { get; set; }

        public List<CQPApp> Apps { get; } = new List<CQPApp>();

        private CQAppManager()
        {
            NativeFunctions.StdCallProcessor = this;
        }

        /// <summary>
        /// 加载文件夹内所有的 <see cref="CQPApp"/>
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="dataDirectory"></param>
        public void LoadApps(string directory,string dataDirectory)
        {
            if (!Directory.Exists(dataDirectory))
                Directory.CreateDirectory(dataDirectory);
            foreach (var path in Directory.GetDirectories(directory))
            {
                var files = Directory.GetFiles(path);
                var dll = files.FirstOrDefault(v => Path.GetExtension(v) == ".dll");
                var json = files.FirstOrDefault(v => Path.GetExtension(v) == ".json");
                if (dll == null || json == null)
                {
                    LogHelper.Write("酷Q应用管理器", "应用加载", $"非法的App: {Path.GetFileName(path)}", LogLevel.Warning);
                    continue;
                }
                try
                {
                    var app = new CQPApp(dll, json, Api);
                    if (app.AppInfo.ResultCode != 1)
                    {
                        LogHelper.Write("酷Q应用管理器", "应用加载", $"解析 {dll} 失败.", LogLevel.Warning);
                        app.Dispose();
                        continue;
                    }
                    var data = Path.Combine(dataDirectory, app.AppInfo.Name);
                    if (!Directory.Exists(data))
                        Directory.CreateDirectory(data);
                    app.DataPath = data;
                    Apps.Add(app);
                    LogHelper.Write("酷Q应用管理器", "应用加载", $"成功加载 {dll} !", LogLevel.InfoSuccess);
                }
                catch (Exception ex)
                {
                    LogHelper.Error("酷Q应用管理器", "应用加载", $"加载 {dll} 失败.", ex);
                }
            }
        }

        /// <summary>
        /// 启用所有已加载的应用
        /// </summary>
        public void EnableApps()
        {
            foreach (var app in Apps.Where(v=>!v.Enabled))
            {
                try
                {
                    var ran = new Random();
                    app.InitializeApp(ran.Next(1000000));
                    app.StartupApp();
                    app.EnableApp();
                    if (app.Enabled)
                        LogHelper.Write("酷Q应用管理器", "应用启用", $"应用 {app.AppInfo.Name} 已启用.", LogLevel.InfoSuccess);
                }
                catch (Exception ex)
                {
                    LogHelper.Error("酷Q应用管理器", "应用启用",$"启用应用 {app.AppInfo.Name} 时发生错误.",ex);
                }
            }
        }

        /// <summary>
        /// 停用所有已加载的应用
        /// </summary>
        public void DisableApps()
        {
            foreach (var app in Apps.Where(v=>v.Enabled))
            {
                try
                {
                    app.DisableApp();
                    LogHelper.Write("酷Q应用管理器", "应用禁用", $"应用 {app.AppInfo.Name} 已禁用.", LogLevel.InfoSuccess);
                }
                catch (Exception ex)
                {
                    LogHelper.Error("酷Q应用管理器", "应用禁用", $"启用应用 {app.AppInfo.Name} 时发生错误.", ex);
                }
            }
        }

        /// <summary>
        /// 向所有已启用的应用分发 PrivateMessage 事件
        /// </summary>
        /// <param name="subType"></param>
        /// <param name="msgId"></param>
        /// <param name="uin"></param>
        /// <param name="msg"></param>
        /// <param name="font"></param>
        [HandleProcessCorruptedStateExceptions]
        public void InvokePrivateMsgEvent(int subType, int msgId, long uin, string msg, int font)
        {
            foreach (var app in Apps.Where(v=>v.Enabled))
            {
                var e = app.AppInfo.Events.FirstOrDefault(v => v.Type == AppEventType.PrivateMessage);
                if(e == default) continue;
                var msgHandle = msg.GetGCHandle();
                try
                {
                    app.GetFunction<Events.PrivateMessageEvent>(e.Function)(subType, msgId, uin,
                        msgHandle.AddrOfPinnedObject(), font);
                }
                catch (Exception ex)
                {
                    LogHelper.Error("酷Q应用管理器", "事件循环", $"向应用 {app.AppInfo.Name} 分发事件 [PrivateMessage] 时出现错误.", ex);
                }
                finally
                {
                    msgHandle.Free();
                }
            }
        }

        /// <summary>
        /// 向所有已启用的应用分发 GroupMessage 事件
        /// </summary>
        /// <param name="subType"></param>
        /// <param name="msgId"></param>
        /// <param name="groupCode"></param>
        /// <param name="uin"></param>
        /// <param name="anonymous"></param>
        /// <param name="msg"></param>
        /// <param name="font"></param>
        [HandleProcessCorruptedStateExceptions]
        public void InvokeGroupMsgEvent(int subType, int msgId, long groupCode, long uin, string anonymous, string msg, int font)
        {
            foreach (var app in Apps.Where(v => v.Enabled))
            {
                var e = app.AppInfo.Events.FirstOrDefault(v => v.Type == AppEventType.GroupMessage);
                if (e == default) continue;
                var anonymousHandle = anonymous.GetGCHandle();
                var msgHandle = msg.GetGCHandle();
                try
                {
                    app.GetFunction<Events.GroupMessageEvent>(e.Function)(subType, msgId, groupCode, uin,
                        anonymousHandle.AddrOfPinnedObject(), msgHandle.AddrOfPinnedObject(), font);
                }
                catch (Exception ex)
                {
                    LogHelper.Error("酷Q应用管理器", "事件循环", $"向应用 {app.AppInfo.Name} 分发事件 [GroupMessage] 时出现错误.", ex);
                }
                finally
                {
                    anonymousHandle.Free();
                    msgHandle.Free();
                }
            }
        }

        /// <summary>
        /// 向所有已启用的应用分发 GroupUpload 事件
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="uin"></param>
        /// <param name="time"></param>
        /// <param name="file"></param>
        [HandleProcessCorruptedStateExceptions]
        public void InvokeGroupUploadEvent(long groupCode, long uin,long time, string file)
        {
            foreach (var app in Apps.Where(v => v.Enabled))
            {
                var e = app.AppInfo.Events.FirstOrDefault(v => v.Type == AppEventType.GroupFileUpload);
                if (e == default) continue;
                var fileHandle = file.GetGCHandle();
                try
                {
                    app.GetFunction<Events.GroupUploadEvent>(e.Function)(1, (int)time, groupCode, uin,
                        fileHandle.AddrOfPinnedObject());
                }
                catch (Exception ex)
                {
                    LogHelper.Error("酷Q应用管理器", "事件循环", $"向应用 {app.AppInfo.Name} 分发事件 [GroupUpload] 时出现错误.", ex);
                }
                finally
                {
                    fileHandle.Free();
                }
            }
        }



        public object Process(int authCode, string func = null, params object[] @params)
        {
            var app = Apps.FirstOrDefault(v =>v.AuthCode == authCode);
            if (app == null || func == null)
                return -998; // 非法调用
            var method = typeof(CQPApp).GetMethod(func, BindingFlags.Public | BindingFlags.Instance);
            return method?.Invoke(app, @params) ?? -2001;
        }
    }
}
