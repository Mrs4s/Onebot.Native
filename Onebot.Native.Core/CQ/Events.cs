using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onebot.Native.Core.CQ
{
    public class Events
    {
        // ---- 酷Q原有事件 ----

        /// <summary>
        /// 初始化APP
        /// </summary>
        /// <param name="authCode"></param>
        /// <returns></returns>
        internal delegate int InitializeFunc(int authCode);

        /// <summary>
        /// CQP启动时调用
        /// </summary>
        /// <returns></returns>
        internal delegate int StartupFunc();

        /// <summary>
        /// CQP退出时调用
        /// </summary>
        /// <returns></returns>
        internal delegate int ExitFunc();

        /// <summary>
        /// 点击按钮
        /// </summary>
        /// <returns></returns>
        internal delegate int MenuFunc();

        /// <summary>
        /// 应用装载事件
        /// </summary>
        /// <returns></returns>
        internal delegate int AppEnableEvent();

        /// <summary>
        /// 应用卸载事件
        /// </summary>
        /// <returns></returns>
        internal delegate int AppDisableEvent();

        /// <summary>
        /// 私聊消息事件
        /// </summary>
        /// <param name="subType">子类型, 11好友；1在线状态；2群；3讨论组。</param>
        /// <param name="msgId">消息ID</param>
        /// <param name="uin">消息来源</param>
        /// <param name="msg">消息内容</param>
        /// <param name="font">字体</param>
        /// <returns></returns>
        internal delegate int PrivateMessageEvent(int subType, int msgId, long uin, IntPtr msg, int font);

        /// <summary>
        /// 群消息事件
        /// </summary>
        /// <param name="subType">子类型, 恒定为1</param>
        /// <param name="msgId">消息ID</param>
        /// <param name="groupCode">来源群</param>
        /// <param name="uin">来源QQ</param>
        /// <param name="anonymous">匿名信息</param>
        /// <param name="msg">消息内容</param>
        /// <param name="font">字体</param>
        /// <returns></returns>
        internal delegate int GroupMessageEvent(int subType, int msgId, long groupCode, long uin, IntPtr anonymous,
            IntPtr msg, int font);


        /// <summary>
        /// 群文件上传事件
        /// </summary>
        /// <param name="subType"></param>
        /// <param name="sendTime"></param>
        /// <param name="groupCode"></param>
        /// <param name="uin"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        internal delegate int GroupUploadEvent(int subType, int sendTime, long groupCode, long uin, IntPtr file);



    }
}
