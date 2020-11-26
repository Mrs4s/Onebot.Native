using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Onebot.Native.Core.Extensions;

namespace Onebot.Native.Core.CQ
{
    public class CQPApp : IDisposable
    {
        private IntPtr _hModule;
        private bool _disposed;
        private readonly ICQPApi _api;

        public string LibraryFile { get; }

        public string MetaFile { get; }

        public string DataPath { get; set; }

        public bool Initialized { get; private set; }

        public bool Enabled { get; private set; }

        public AppInfo AppInfo { get;}

        public int AuthCode { get; private set; }


        public CQPApp(string dllFile, string jsonFile, ICQPApi api)
        {
            if (!File.Exists(dllFile))
                throw new FileNotFoundException(dllFile + " 不存在.");
            if(!File.Exists(jsonFile))
                throw new FileNotFoundException(jsonFile + " 不存在.");
            LibraryFile = dllFile;
            MetaFile = jsonFile;
            _hModule = Win32.LoadLibraryA(dllFile);
            if (_hModule == IntPtr.Zero)
                throw new FileLoadException($"无法加载 {dllFile}: {Win32.GetLastError()}");
            AppInfo = JsonConvert.DeserializeObject<AppInfo>(File.ReadAllText(jsonFile));
            _api = api;
        }

        // ---- 初始化 ----

        public int InitializeApp(int authCode)
        {
            var code = GetFunction<Events.InitializeFunc>("Initialize")(authCode);
            if (code == 0) Initialized = true;
            AuthCode = authCode;
            return code;
        }

        public int StartupApp()
        {
            var startup = AppInfo.Events.FirstOrDefault(v => v.Type == AppEventType.Startup);
            if (startup == default) return 0;
            return GetFunction<Events.StartupFunc>(startup.Function)();
        }

        public int EnableApp()
        {
            var enable = AppInfo.Events.FirstOrDefault(v => v.Type == AppEventType.AppEnable);
            if (enable == default) throw new MissingMethodException("启用函数不存在.");
            var code = GetFunction<Events.AppEnableEvent>(enable.Function)();
            if (code == 0) Enabled = true;
            return code;
        }

        public int DisableApp()
        {
            var disable = AppInfo.Events.FirstOrDefault(v => v.Type == AppEventType.AppDisable);
            if(disable == default) throw new MissingMethodException("停用函数不存在.");
            var code = GetFunction<Events.AppDisableEvent>(disable.Function)();
            if (code == 0) Enabled = false;
            return code;
        }

        [HandleProcessCorruptedStateExceptions]
        public void InvokeMenu(string name)
        {
            try
            {
                var menu = AppInfo.Menus.FirstOrDefault(v => v.Name == name);
                if (menu == default) return;
                GetFunction<Events.MenuFunc>(menu.Function)();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }


        // ---- 处理stdcall ----

        public int AddLog(int level, string type, string content)
        {
            LogHelper.Write(AppInfo.Name, type, content, (LogLevel)level);
            return 0;
        }

        public string GetAppDirectory() => Path.GetFullPath(DataPath);

        public string GetLoginNick() => _api.GetLoginNick();

        // ReSharper disable once InconsistentNaming
        public long GetLoginQQ() => _api.GetLoginQQ();

        public int SendPrivateMsg(long account, string msg) => _api.SendPrivateMsg(account, msg).Also(_ =>
            LogHelper.Write(AppInfo.Name, "[↑]私聊消息", $"向 [私聊: {account}] 发送消息: {msg}", LogLevel.InfoSend));

        public int SendGroupMsg(long groupCode, string msg) => _api.SendGroupMsg(groupCode, msg)
            .Also(_ => LogHelper.Write(AppInfo.Name, "[↑]群组消息", $"向 [群: {groupCode}] 发送消息: {msg}", LogLevel.InfoSend));

        public string GetGroupList() => _api.GetGroupList();

        public string GetGroupInfo(long groupCode, bool cache) => _api.GetGroupInfo(groupCode);

        public string GetGroupMemberList(long groupCode) => _api.GetGroupMemberList(groupCode);

        public string GetGroupMemberInfo(long groupCode, long account, bool cache) =>
            _api.GetGroupMemberInfo(groupCode, account);

        public int DeleteMsg(long msgId) => _api.DeleteMsg(msgId);

        public int SetGroupKick(long groupCode, long account, bool block) =>
            _api.SetGroupKick(groupCode, account, block);

        public int SetGroupBan(long groupCode, long account, long time) => _api.SetGroupBan(groupCode, account, time);

        public int SetGroupAdmin(long groupCode, long account, bool set) => _api.SetGroupAdmin(groupCode, account, set);

        public int SetGroupSpecialTitle(long groupCode, long account, string title, long time) =>
            _api.SetGroupSpecialTitle(groupCode, account, title, time);

        public int SetGroupWholeBan(long groupCode, bool set) => _api.SetGroupWholeBan(groupCode, set);

        public int SetGroupCard(long groupCode, long account, string card) =>
            _api.SetGroupCard(groupCode, account, card);




        // ---- 处理指针回调 ----

        /// <summary>
        /// 获取指定函数指针并转换为委托
        /// </summary>
        /// <typeparam name="T">委托类型</typeparam>
        /// <param name="name">函数名</param>
        /// <returns></returns>
        public T GetFunction<T>(string name) where T : Delegate =>
            Marshal.GetDelegateForFunctionPointer<T>(GetFunctionPtr(name));


        /// <summary>
        /// 返回指定函数指针
        /// </summary>
        /// <param name="name">函数名</param>
        /// <returns></returns>
        private IntPtr GetFunctionPtr(string name)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(CQPApp));
            var ptr = Win32.GetProcAddress(_hModule, name);
            if (ptr == IntPtr.Zero)
                throw new EntryPointNotFoundException($"函数 {name} 不存在.");
            return ptr;
        }


        ~CQPApp() => Dispose();

        public void Dispose()
        {
            if (_hModule == IntPtr.Zero) return;
            Win32.FreeLibrary(_hModule);
            _hModule = IntPtr.Zero;
            _disposed = true;
        }
    }
}
