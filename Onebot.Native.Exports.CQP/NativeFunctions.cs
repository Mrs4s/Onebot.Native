using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Onebot.Native.Exports.CQP
{
    public static class NativeFunctions
    {
        private static readonly Encoding _encoding = Encoding.GetEncoding("GB18030");

        public static IFuncProcessor StdCallProcessor { get; set; }

        [DllImport("kernel32.dll", EntryPoint = "lstrlenA", CharSet = CharSet.Ansi)]
        private static extern int LStrLenA(IntPtr ptr);

        private static string ToManagedString(this IntPtr @this)
        {
            if (@this == IntPtr.Zero)
                return string.Empty;
            var len = LStrLenA(@this);
            if (len == 0) return string.Empty;
            var buf = new byte[len];
            Marshal.Copy(@this, buf, 0, len);
            return _encoding.GetString(buf);
        }

        public static GCHandle GetGCHandle(this string @this)
        {
            var buf = _encoding.GetBytes(@this);
            return GCHandle.Alloc(buf, GCHandleType.Pinned);
        }


        [DllExport("CQ_sendPrivateMsg", CallingConvention = CallingConvention.StdCall)]
        public static int SendPrivateMsg(int auth, long account, IntPtr msg) =>
            StdCallProcessor?.Process(auth, nameof(SendPrivateMsg), account, msg.ToManagedString()) as int? ?? -2001;

        [DllExport("CQ_sendGroupMsg", CallingConvention = CallingConvention.StdCall)]
        public static int SendGroupMsg(int auth, long groupCode, IntPtr msg) =>
            StdCallProcessor?.Process(auth, nameof(SendGroupMsg), groupCode, msg.ToManagedString()) as int? ?? -2001;

        [DllExport("CQ_deleteMsg", CallingConvention = CallingConvention.StdCall)]
        public static int DeleteMsg(int auth, long msgId) =>
            StdCallProcessor?.Process(auth, nameof(DeleteMsg), msgId) as int? ?? -2001;

        [DllExport("CQ_setGroupKick", CallingConvention = CallingConvention.StdCall)]
        public static int SetGroupKick(int auth, long groupCode, long account, bool block) =>
            StdCallProcessor?.Process(auth, nameof(SetGroupKick), groupCode, account, block) as int? ?? -2001;

        [DllExport("CQ_setGroupBan", CallingConvention = CallingConvention.StdCall)]
        public static int SetGroupBan(int auth, long groupCode, long account, long time) =>
            StdCallProcessor?.Process(auth, nameof(SetGroupBan), groupCode, account, time) as int? ?? -2001;

        [DllExport("CQ_setGroupAdmin", CallingConvention = CallingConvention.StdCall)]
        public static int SetGroupAdmin(int auth, long groupCode, long account, bool set) =>
            StdCallProcessor?.Process(auth, nameof(SetGroupAdmin), groupCode, account, set) as int? ?? -2001;

        [DllExport("CQ_setGroupSpecialTitle", CallingConvention = CallingConvention.StdCall)]
        public static int SetGroupSpecialTitle(int auth, long groupCode, long account, IntPtr title, long time) =>
            StdCallProcessor?.Process(auth, nameof(SetGroupSpecialTitle), groupCode, account,
                title.ToManagedString(), time) as int? ?? -2001;

        [DllExport("CQ_setGroupWholeBan", CallingConvention = CallingConvention.StdCall)]
        public static int SetGroupWholeBan(int auth, long groupCode, bool set) =>
            StdCallProcessor?.Process(auth, nameof(SetGroupWholeBan), groupCode, set) as int? ?? -2001;

        [DllExport("CQ_setGroupCard", CallingConvention = CallingConvention.StdCall)]
        public static int SetGroupCard(int auth, long groupCode, long account, IntPtr card) =>
            StdCallProcessor?.Process(auth, nameof(SetGroupCard), groupCode, account, card.ToManagedString()) as
                int? ?? -2001;


        [DllExport("CQ_setGroupLeave", CallingConvention = CallingConvention.StdCall)]
        public static int SetGroupLeave(int auth, long groupCode, bool _) =>
            StdCallProcessor?.Process(auth, nameof(SetGroupLeave), groupCode) as int? ?? -2001;

        [DllExport("CQ_setGroupAddRequestV2", CallingConvention = CallingConvention.StdCall)]
        public static int SetGroupAddRequest(int auth, IntPtr flag, int reqType, int retType, IntPtr msg) =>
            StdCallProcessor?.Process(auth, nameof(SetGroupAddRequest), flag.ToManagedString(), reqType, retType,
                msg.ToManagedString()) as int? ?? -2001;

        [DllExport("CQ_setFriendAddRequest", CallingConvention = CallingConvention.StdCall)]
        public static int SetFriendAddRequest(int auth, IntPtr flag, int type, IntPtr msg) =>
            StdCallProcessor?.Process(auth, nameof(SetFriendAddRequest), flag.ToManagedString(), type,
                msg.ToManagedString()) as int? ?? -2001;


        [DllExport("CQ_getGroupList", CallingConvention = CallingConvention.StdCall)]
        public static IntPtr GetGroupList(int auth)
        {
            var ret = StdCallProcessor?.Process(auth, nameof(GetGroupList)) as string;
            if (string.IsNullOrEmpty(ret))
                return IntPtr.Zero;
            var handle = ret.GetGCHandle();
            try
            {
                return handle.AddrOfPinnedObject();
            }
            finally
            {
                handle.Free();
            }
        }

        [DllExport("CQ_getGroupMemberList", CallingConvention = CallingConvention.StdCall)]
        public static IntPtr GetGroupMemberList(int auth, long groupCode)
        {
            var ret = StdCallProcessor?.Process(auth, nameof(GetGroupMemberList), groupCode) as string;
            if (string.IsNullOrEmpty(ret))
                return IntPtr.Zero;
            var handle = ret.GetGCHandle();
            try
            {
                return handle.AddrOfPinnedObject();
            }
            finally
            {
                handle.Free();
            }
        }

        [DllExport("CQ_getGroupMemberInfoV2", CallingConvention = CallingConvention.StdCall)]
        public static IntPtr GetGroupMemberInfo(int auth, long groupCode, long account, bool cache)
        {
            var ret = StdCallProcessor?.Process(auth, nameof(GetGroupMemberInfo), groupCode, account, cache) as string;
            if (string.IsNullOrEmpty(ret))
                return IntPtr.Zero;
            var handle = ret.GetGCHandle();
            try
            {
                return handle.AddrOfPinnedObject();
            }
            finally
            {
                handle.Free();
            }
        }


        [DllExport("CQ_getFriendList", CallingConvention = CallingConvention.StdCall)]
        public static IntPtr GetFriendList(int auth, bool reserved)
        {
            var ret = StdCallProcessor?.Process(auth, nameof(GetFriendList), reserved) as string;
            if (string.IsNullOrEmpty(ret))
                return IntPtr.Zero;
            var handle = ret.GetGCHandle();
            try
            {
                return handle.AddrOfPinnedObject();
            }
            finally
            {
                handle.Free();
            }
        }

        [DllExport("CQ_getStrangerInfo", CallingConvention = CallingConvention.StdCall)]
        public static IntPtr GetStrangerInfo(int auth, long account, bool cache)
        {
            var ret = StdCallProcessor?.Process(auth, nameof(GetStrangerInfo), account, cache) as string;
            if (string.IsNullOrEmpty(ret))
                return IntPtr.Zero;
            var handle = ret.GetGCHandle();
            try
            {
                return handle.AddrOfPinnedObject();
            }
            finally
            {
                handle.Free();
            }
        }

        [DllExport("CQ_getGroupInfo", CallingConvention = CallingConvention.StdCall)]
        public static IntPtr GetGroupInfo(int auth, long groupCode, bool cache)
        {
            var ret = StdCallProcessor?.Process(auth, nameof(GetGroupInfo), groupCode, cache) as string;
            if (string.IsNullOrEmpty(ret))
                return IntPtr.Zero;
            var handle = ret.GetGCHandle();
            try
            {
                return handle.AddrOfPinnedObject();
            }
            finally
            {
                handle.Free();
            }
        }


        [DllExport("CQ_getLoginQQ", CallingConvention = CallingConvention.StdCall)]
        public static long GetLoginQQ(int auth) =>
            StdCallProcessor?.Process(auth, nameof(GetLoginQQ)) as long? ?? -2001;

        [DllExport("CQ_getLoginNick", CallingConvention = CallingConvention.StdCall)]
        public static IntPtr GetLoginNick(int auth)
        {
            var nick = StdCallProcessor?.Process(auth, nameof(GetLoginNick)) as string;
            if (string.IsNullOrEmpty(nick))
                return IntPtr.Zero;
            var handle = nick.GetGCHandle();
            try
            {
                return handle.AddrOfPinnedObject();
            }
            finally
            {
                handle.Free();
            }
        }

        [DllExport("CQ_addLog", CallingConvention = CallingConvention.StdCall)]
        public static int AddLog(int auth, int level, IntPtr type, IntPtr content) =>
            StdCallProcessor?.Process(auth, nameof(AddLog), level, type.ToManagedString(),
                content.ToManagedString()) as int? ?? -2001;

        [DllExport("CQ_getAppDirectory", CallingConvention = CallingConvention.StdCall)]
        public static IntPtr GetAppDirectory(int auth)
        {
            var path = StdCallProcessor?.Process(auth, nameof(GetAppDirectory)) as string;
            if (string.IsNullOrEmpty(path))
                return IntPtr.Zero;
            var handle = path.GetGCHandle();
            try
            {
                return handle.AddrOfPinnedObject();
            }
            finally
            {
                handle.Free();
            }
        }

        [DllExport("CQ_canSendImage", CallingConvention = CallingConvention.StdCall)]
        public static int CanSendImage(int auth) => 1;

        [DllExport("CQ_canSendRecord", CallingConvention = CallingConvention.StdCall)]
        public static int CanSendRecord(int auth) => 1;

        // ---- 不兼容函数 ----

        [DllExport("CQ_getCsrfToken", CallingConvention = CallingConvention.StdCall)]
        public static int GetCSRFToken(int auth) => 0;

        [DllExport("CQ_sendDiscussMsg", CallingConvention = CallingConvention.StdCall)]
        public static int SendDiscussMsg(int auth, long discussCode, IntPtr msg) => 0;

        [DllExport("CQ_setDiscussLeave", CallingConvention = CallingConvention.StdCall)]
        public static int SetDiscussLeave(int auth, long code) => 0;

        [DllExport("CQ_sendLikeV2", CallingConvention = CallingConvention.StdCall)]
        public static int SendLike(int auth, long account, int times) => 0;

        [DllExport("CQ_getCookiesV2", CallingConvention = CallingConvention.StdCall)]
        public static IntPtr GetCookies(int auth, IntPtr domain) => IntPtr.Zero;


    }
}
