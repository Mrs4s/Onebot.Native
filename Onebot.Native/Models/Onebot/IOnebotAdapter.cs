using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Onebot.Native.Models.Onebot
{
    /// <summary>
    /// Onebot Api 接口
    /// </summary>
    public interface IOnebotAdapter
    {
        Task Run();

        // --- Events ---
        // 交给上层解析

        event Action<JToken> OnGroupMessage;

        event Action<JToken> OnPrivateMessage;

        event Action<JToken> OnGroupUpload;

        // --- Apis ---

        /// <summary>
        /// 获取登录账号信息
        /// </summary>
        /// <returns></returns>
        Task<LoginInfo> GetLoginInfoAsync();

        /// <summary>
        /// 发送私聊消息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="msg"></param>
        /// <returns>MsgId</returns>
        Task<int> SendPrivateMsgAsync(long account, string msg);

        /// <summary>
        /// 发送群聊消息
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="msg"></param>
        /// <returns>MsgId</returns>
        Task<int> SendGroupMsgAsync(long groupCode, string msg);

        /// <summary>
        /// 撤回消息
        /// </summary>
        /// <param name="msgId"></param>
        /// <returns></returns>
        Task<bool> DeleteMsgAsync(long msgId);

        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<FriendInfo>> GetFriendListAsync();

        /// <summary>
        /// 获取群列表
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<GroupInfo>> GetGroupListAsync();

        /// <summary>
        /// 获取群信息
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        Task<GroupInfo> GetGroupInfoAsync(long groupCode);

        /// <summary>
        /// 获取群员列表
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="cache"></param>
        /// <returns></returns>
        Task<IEnumerable<GroupMemberInfo>> GetGroupMembersAsync(long groupCode, bool cache);

        /// <summary>
        /// T出群员
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="account"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        Task<bool> SetGroupKickAsync(long groupCode, long account, bool block);

        /// <summary>
        /// 禁言群员
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="account"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        Task<bool> SetGroupBanAsync(long groupCode, long account, long time);

        /// <summary>
        /// 设置群管理
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="account"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        Task<bool> SetGroupAdminAsync(long groupCode, long account, bool set);

        /// <summary>
        /// 设置群头衔
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="account"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        Task<bool> SetGroupSpecialTitleAsync(long groupCode, long account, string title);

        /// <summary>
        /// 全体禁言
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        Task<bool> SetGroupWholeBanAsync(long groupCode, bool set);

        /// <summary>
        /// 设置群名片
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="account"></param>
        /// <param name="card"></param>
        /// <returns></returns>
        Task<bool> SetGroupCardAsync(long groupCode, long account, string card);

        /// <summary>
        /// 退群
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        Task<bool> SetGroupLeaveAsync(long groupCode);

        /// <summary>
        /// 处理加群请求
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="reqType"></param>
        /// <param name="retType"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task<bool> SetGroupAddRequestAsync(string flag, int reqType, int retType, string msg);

        /// <summary>
        /// 处理好友请求
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task<bool> SetFriendAddRequestAsync(string flag, int type, string msg);
    }
}
