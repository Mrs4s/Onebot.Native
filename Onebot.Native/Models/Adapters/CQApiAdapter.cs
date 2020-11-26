using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Onebot.Native.Core.CQ;
using Onebot.Native.Core.Extensions;
using Onebot.Native.Core.Models;
using Onebot.Native.Models.Onebot;

namespace Onebot.Native.Models.Adapters
{
    public sealed class CQApiAdapter : ICQPApi
    {
        private readonly IOnebotAdapter _onebot;

        public CQApiAdapter(IOnebotAdapter onebot)
        {
            _onebot = onebot;
        }


        public string GetLoginNick() => _onebot.GetLoginInfoAsync().GetAwaiter().GetResult()?.Nickname ?? string.Empty;

        public long GetLoginQQ() => _onebot.GetLoginInfoAsync().GetAwaiter().GetResult()?.UserId ?? 0;

        public int SendPrivateMsg(long account, string msg) =>
            _onebot.SendPrivateMsgAsync(account, msg).GetAwaiter().GetResult();

        public int SendGroupMsg(long groupCode, string msg) =>
            _onebot.SendGroupMsgAsync(groupCode, msg).GetAwaiter().GetResult();

        public string GroupFriendList()
        {
            var list = _onebot.GetFriendListAsync().GetAwaiter().GetResult()?.ToList();
            if (list == null)
                return string.Empty;
            using (var ms = new MemoryStream())
            using (var packet = new BytesPacket(ms))
            {
                packet.WriteInt32(list.Count);
                list.ForEach(info => packet.WriteShortPacket(p =>
                    p.WriteInt64(info.Userid).WriteString(info.Nickname).WriteString(info.Remark)));
                return packet.ToBase64();
            }
        }

        public string GetGroupList()
        {
            var list = _onebot.GetGroupListAsync().GetAwaiter().GetResult()?.ToList();
            if (list == null)
                return string.Empty;
            using (var ms = new MemoryStream())
            using (var packet = new BytesPacket(ms))
            {
                packet.WriteInt32(list.Count);
                list.ForEach(info =>
                    packet.WriteShortPacket(p => p.WriteInt64(info.GroupId).WriteString(info.GroupName)));
                return packet.ToBase64();
            }
        }

        public string GetGroupInfo(long groupCode)
        {
            var info = _onebot.GetGroupInfoAsync(groupCode).GetAwaiter().GetResult();
            if (info == default)
                return string.Empty;
            using (var ms = new MemoryStream())
            using (var packet = new BytesPacket(ms))
            {
                return packet
                    .WriteInt64(info.GroupId)
                    .WriteString(info.GroupName)
                    .WriteInt32(info.MemberCount)
                    .WriteInt32(info.MaxMemberCount)
                    .ToBase64();
            }
        }

        public string GetGroupMemberList(long groupCode)
        {
            var list = _onebot.GetGroupMembersAsync(groupCode, true).GetAwaiter().GetResult()?.ToList();
            if (list == null)
                return string.Empty;
            using (var ms = new MemoryStream())
            using (var packet = new BytesPacket(ms))
            {
                packet.WriteInt32(list.Count);
                list.ForEach(info => WriteMemberInfo(packet, info));
                return packet.ToBase64();
            }
        }

        public string GetGroupMemberInfo(long groupCode, long account)
        {
            var info = _onebot.GetGroupMembersAsync(groupCode, true).GetAwaiter().GetResult()?.ToList()
                .FirstOrDefault(v => v.Userid == account);
            if (info == null)
                return string.Empty;
            using (var ms = new MemoryStream())
            using (var packet = new BytesPacket(ms))
                return packet.Also(p => WriteMemberInfo(p, info)).ToBase64();
        }

        public int DeleteMsg(long msgId) => _onebot.DeleteMsgAsync(msgId).GetAwaiter().GetResult() ? 0 : -1;

        public int SetGroupKick(long groupCode, long account, bool block) =>
            _onebot.SetGroupKickAsync(groupCode, account, block).GetAwaiter().GetResult() ? 0 : -1;

        public int SetGroupBan(long groupCode, long account, long time) =>
            _onebot.SetGroupBanAsync(groupCode, account, time).GetAwaiter().GetResult() ? 0 : -1;

        public int SetGroupAdmin(long groupCode, long account, bool set) =>
            _onebot.SetGroupAdminAsync(groupCode, account, set).GetAwaiter().GetResult() ? 0 : -1;

        public int SetGroupSpecialTitle(long groupCode, long account, string title, long time) =>
            _onebot.SetGroupSpecialTitleAsync(groupCode, account, title).GetAwaiter().GetResult() ? 0 : -1;

        public int SetGroupWholeBan(long groupCode, bool set) =>
            _onebot.SetGroupWholeBanAsync(groupCode, set).GetAwaiter().GetResult() ? 0 : -1;

        public int SetGroupCard(long groupCode, long account, string card) =>
            _onebot.SetGroupCardAsync(groupCode, account, card).GetAwaiter().GetResult() ? 0 : -1;

        public int SetGroupLeave(long groupCode) =>
            _onebot.SetGroupLeaveAsync(groupCode).GetAwaiter().GetResult() ? 0 : -1;

        public int SetGroupAddRequest(string flag, int reqType, int retType, string msg) =>
            _onebot.SetGroupAddRequestAsync(flag, reqType, retType, msg).GetAwaiter().GetResult() ? 0 : -1;

        public int SetFriendAddRequest(string flag, int type, string msg)
        {
            throw new NotImplementedException();
        }

        private static void WriteMemberInfo(BytesPacket packet, GroupMemberInfo info) => packet.WriteShortPacket(p => p
            .WriteInt64(info.GroupId)
            .WriteInt64(info.Userid)
            .WriteString(info.Nickname)
            .WriteString(info.CardName)
            .WriteInt32(0)
            .WriteInt32(info.Age)
            .WriteString(info.Area)
            .WriteInt32((int)info.JoinTime)
            .WriteInt32((int)info.LastSentTime)
            .WriteString(info.Title)
            .WriteInt32(info.Role == "owner" ? 3 : info.Role == "admin" ? 2 : 1)
            .WriteBool(false)
            .WriteString(info.Title)
            .WriteInt32((int)info.TitleExpireTime)
            .WriteBool(true)
        );
    }
}
