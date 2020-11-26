using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onebot.Native.Core.CQ
{
    /// <summary>
    /// 用于处理来自CQP插件的API请求
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public interface ICQPApi
    {
        /// <summary>
        /// 获取登录账号昵称
        /// </summary>
        /// <returns></returns>
        string GetLoginNick();

        /// <summary>
        /// 获取登录账号UIN
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        long GetLoginQQ();

        /// <summary>
        /// 发送私聊信息
        /// </summary>
        /// <param name="account">目标账号</param>
        /// <param name="msg">信息</param>
        /// <returns>消息ID</returns>
        int SendPrivateMsg(long account, string msg);

        /// <summary>
        /// 发送群聊信息
        /// </summary>
        /// <param name="groupCode">目标群</param>
        /// <param name="msg">信息</param>
        /// <returns>消息ID</returns>
        int SendGroupMsg(long groupCode, string msg);


        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <returns></returns>
        string GroupFriendList();

        /// <summary>
        /// 获取群列表
        /// </summary>
        /// <returns></returns>
        string GetGroupList();

        /// <summary>
        /// 获取群组信息
        /// </summary>
        /// <returns></returns>
        string GetGroupInfo(long groupCode);

        /// <summary>
        /// 获取群成员列表
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        string GetGroupMemberList(long groupCode);

        /// <summary>
        /// 获取群成员信息
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        string GetGroupMemberInfo(long groupCode, long account);


        /// <summary>
        /// 撤回消息
        /// </summary>
        /// <param name="msgId">消息ID</param>
        /// <returns></returns>
        int DeleteMsg(long msgId);

        /// <summary>
        /// T出群员
        /// </summary>
        /// <param name="groupCode">目标群</param>
        /// <param name="account">目标UIN</param>
        /// <param name="block">是否屏蔽</param>
        /// <returns></returns>
        int SetGroupKick(long groupCode, long account, bool block);

        /// <summary>
        /// 禁言群成员
        /// </summary>
        /// <param name="groupCode">目标群</param>
        /// <param name="account">目标UIN</param>
        /// <param name="time">时长</param>
        /// <returns></returns>
        int SetGroupBan(long groupCode, long account, long time);


        /// <summary>
        /// 设置群管理
        /// </summary>
        /// <param name="groupCode">目标群</param>
        /// <param name="account">目标UIN</param>
        /// <param name="set">设置或取消</param>
        /// <returns></returns>
        int SetGroupAdmin(long groupCode, long account, bool set);

        /// <summary>
        /// 设置群头衔
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="account"></param>
        /// <param name="title"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        int SetGroupSpecialTitle(long groupCode, long account, string title, long time);

        /// <summary>
        /// 设置全体禁言
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        int SetGroupWholeBan(long groupCode, bool set);

        /// <summary>
        /// 设置群名片
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="account"></param>
        /// <param name="card"></param>
        /// <returns></returns>
        int SetGroupCard(long groupCode, long account, string card);

        /// <summary>
        /// 退群
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        int SetGroupLeave(long groupCode);

        int SetGroupAddRequest(string flag, int reqType, int retType, string msg);

        int SetFriendAddRequest(string flag, int type, string msg);




    }
}
