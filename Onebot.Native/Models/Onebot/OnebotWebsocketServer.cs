using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Onebot.Native.Core;
using Onebot.Native.Core.CQ;
using Onebot.Native.Core.Extensions;
using Onebot.Native.Models.Adapters;
using LogLevel = Onebot.Native.Core.LogLevel;

namespace Onebot.Native.Models.Onebot
{
    public class OnebotWebsocketServer : IOnebotAdapter
    {
        public string Address { get; }

        public WebSocketServer Server { get; }

        public string AccessToken { get; }

        public long Uin { get; private set; }

        public event Action<OnebotWebsocketServer> OnConnected;

        public event Action<JToken> OnGroupMessage;

        public event Action<JToken> OnPrivateMessage;

        public event Action<JToken> OnGroupUpload; 

        private IWebSocketConnection _connection;
        private readonly Dictionary<string, Action<string>> _requests = new Dictionary<string, Action<string>>();

        public OnebotWebsocketServer(string address, string accessToken = null)
        {
            Server = new WebSocketServer(address);
            Address = address;
            AccessToken = accessToken;
        }

        public Task Run()
        {
            Server.Start(socket =>
            {
                if (!socket.ConnectionInfo.Headers.TryGetValue("X-Self-ID", out var self))
                    return;
                if (!socket.ConnectionInfo.Headers.TryGetValue("X-Client-Role", out var role))
                    return;
                if (!string.IsNullOrEmpty(AccessToken))
                {
                    if (!socket.ConnectionInfo.Headers.TryGetValue("Authorization", out var token))
                        return;
                    if (AccessToken != token)
                    {
                        LogHelper.Write("OnebotServer", "连接处理", $"断开来自 {socket.ConnectionInfo.ClientIpAddress}:{socket.ConnectionInfo.ClientPort} 的连接: Token校验失败.", LogLevel.Warning);
                        socket.Close();
                        return;
                    }
                }
                if (role != "Universal")
                {
                    LogHelper.Write("OnebotServer", "连接处理", $"来自 {socket.ConnectionInfo.ClientIpAddress}:{socket.ConnectionInfo.ClientPort} 的未知连接. 请确保使用的 Universal 上报.", LogLevel.Warning);
                    socket.Close();
                    return;
                }
                socket.OnOpen = () =>
                {
                    var flag = false;
                    if (Uin == default)
                    {
                        flag = true;
                        Uin = long.Parse(self);
                    }
                        
                    if (Uin != long.Parse(self))
                    {
                        LogHelper.Write("OnebotServer", "连接处理", $"断开来自 {socket.ConnectionInfo.ClientIpAddress}:{socket.ConnectionInfo.ClientPort} 的连接: 只能同时处理一个连接.", LogLevel.Warning);
                        socket.Close();
                        return;
                    }
                    LogHelper.Write("OnebotServer", "连接处理", $"已接收 {socket.ConnectionInfo.ClientIpAddress}:{socket.ConnectionInfo.ClientPort} 的连接.", LogLevel.InfoSuccess);
                    _connection = socket;
                    if (flag)
                        OnConnected?.Invoke(this);
                };

                socket.OnMessage = msg =>
                {
                    try
                    {
                        var payload = JToken.Parse(msg);
                        if (payload["echo"] != null)
                        {
                            var echo = payload["echo"].ToString();
                            if (_requests.ContainsKey(echo))
                            {
                                if (payload["status"]?.ToString() == "failed")
                                {
                                    _requests[echo]("failed");
                                    return;
                                }
                                _requests[echo](payload["data"]?.ToString());
                                return;
                            }
                        }
                        // 待测试: 不知道原酷Q是怎么处理事件循环线程的
                        Task.Factory.StartNew(() => EventProcessor(payload));
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("OnebotServer", "事件处理", $"来自 {socket.ConnectionInfo.ClientIpAddress}:{socket.ConnectionInfo.ClientPort} 的事件处理错误.", ex);
                    }
                };
            });
            LogHelper.Write("Websocket", "服务启动", "Onebot Websocket服务已启动: " + Address, LogLevel.InfoSuccess);
            return Task.CompletedTask;
        }

        private async Task<string> RequestAsync(string action, object p = null)
        {
            if (!_connection.IsAvailable)
                return null;
            var echo = StringExtensions.RandomString;
            var reset = new ManualResetEvent(false);
            var rsp = "{}";
            _requests.Add(echo, payload =>
            {
                rsp = payload;
                reset.Set();
            });
            await _connection.Send(JsonConvert.SerializeObject(new
            {
                action,
                echo,
                @params = p
            }));
            reset.WaitOne(TimeSpan.FromSeconds(30));
            reset.Close();
            _requests.Remove(echo);
            if (rsp == "failed")
                throw new MethodAccessException("Api访问错误");
            return rsp;
        }

        private void EventProcessor(JToken payload)
        {
            switch (payload["post_type"]?.ToString())
            {
                case "message":
                    if (payload["message_type"]?.ToString() == "private")
                    {
                        LogHelper.Write("Onebot", "消息处理",
                            $"[↓] 接收到 [私聊: {payload["user_id"]?.ToObject<long>()}] 的消息: {OnebotTools.ParseMessageToString(payload["message"])}",
                            LogLevel.InfoReceive);
                        OnPrivateMessage?.Invoke(payload);
                    }
                    if (payload["message_type"]?.ToString() == "group")
                    {
                        LogHelper.Write("Onebot", "消息处理",
                            $"[↓] 接收到 [群: {payload["group_id"]?.ToObject<long>()} 用户: {payload.SelectToken("sender.user_id")?.ToObject<long>()}] 的消息: {OnebotTools.ParseMessageToString(payload["message"])}",
                            LogLevel.InfoReceive);
                        OnGroupMessage?.Invoke(payload);
                    }
                    break;
                case "notice":
                    if (payload["notice_type"]?.ToString() == "group_upload")
                    {
                        LogHelper.Write("Onebot", "文件上传",
                            $"[↓] 来自 [群: {payload["group_id"]?.ToObject<long>()} 用户: {payload["user_id"]?.ToObject<long>()}] 上传了文件: {payload.SelectToken("file.name")}",
                            LogLevel.InfoReceive);
                        OnGroupUpload?.Invoke(payload);
                    }
                    break;
            }
        }

        public async Task<LoginInfo> GetLoginInfoAsync() => await RequestAsync("get_login_info").LetAsync(JObject.Parse)
            .LetAsync(v => v.ToObject<LoginInfo>()).RunCatchDefault();

        public async Task<int> SendPrivateMsgAsync(long account, string msg) =>
            await RequestAsync("send_private_msg", new { user_id = account, message = msg }).LetAsync(JToken.Parse)
                .LetAsync(v => v["message_id"]?.ToObject<int>() ?? default).RunCatchDefault();

        public async Task<int> SendGroupMsgAsync(long groupCode, string msg) =>
            await RequestAsync("send_group_msg", new { group_id = groupCode, message = msg }).LetAsync(JToken.Parse)
                .LetAsync(v => v["message_id"]?.ToObject<int>() ?? default).RunCatchDefault();

        public async Task<bool> DeleteMsgAsync(long msgId) =>
            await RequestAsync("delete_msg", new { message_id = msgId }).RunSuccessful();

        public async Task<IEnumerable<FriendInfo>> GetFriendListAsync() => await RequestAsync("get_friend_list")
            .LetAsync(JToken.Parse).LetAsync(v => v.Select(token => token.ToObject<FriendInfo>())).RunCatchDefault();

        public async Task<IEnumerable<GroupInfo>> GetGroupListAsync() => await RequestAsync("get_group_list")
            .LetAsync(JToken.Parse).LetAsync(v => v.Select(token => token.ToObject<GroupInfo>())).RunCatchDefault();

        public async Task<GroupInfo> GetGroupInfoAsync(long groupCode) =>
            await RequestAsync("get_group_info", new { group_id = groupCode }).LetAsync(JToken.Parse)
                .LetAsync(v => v.ToObject<GroupInfo>()).RunCatchDefault();

        public async Task<IEnumerable<GroupMemberInfo>> GetGroupMembersAsync(long groupCode, bool cache) =>
            await RequestAsync("get_group_member_list", new { group_id = groupCode, no_cache = cache })
                .LetAsync(JToken.Parse).LetAsync(v => v.Select(token => token.ToObject<GroupMemberInfo>()))
                .RunCatchDefault();

        public async Task<bool> SetGroupKickAsync(long groupCode, long account, bool block) =>
            await RequestAsync("set_group_kick",
                new { group_id = groupCode, user_id = account, reject_add_request = block }).RunSuccessful();

        public async Task<bool> SetGroupBanAsync(long groupCode, long account, long time) =>
            await RequestAsync("set_group_ban", new { group_id = groupCode, user_id = account, duration = time })
                .RunSuccessful();

        public async Task<bool> SetGroupAdminAsync(long groupCode, long account, bool set) =>
            await RequestAsync("set_group_admin", new { group_id = groupCode, user_id = account, enable = set })
                .RunSuccessful();

        public async Task<bool> SetGroupSpecialTitleAsync(long groupCode, long account, string title) =>
            await RequestAsync("set_group_special_title",
                new { group_id = groupCode, user_id = account, special_title = title }).RunSuccessful();

        public async Task<bool> SetGroupWholeBanAsync(long groupCode, bool set) =>
            await RequestAsync("set_group_whole_ban", new { group_id = groupCode, enable = set }).RunSuccessful();

        public async Task<bool> SetGroupCardAsync(long groupCode, long account, string card) =>
            await RequestAsync("set_group_card", new { group_id = groupCode, user_id = account, card }).RunSuccessful();

        public async Task<bool> SetGroupLeaveAsync(long groupCode) =>
            await RequestAsync("set_group_leave", new { group_id = groupCode }).RunSuccessful();

        public async Task<bool> SetGroupAddRequestAsync(string flag, int reqType, int retType, string msg)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SetFriendAddRequestAsync(string flag, int type, string msg)
        {
            throw new NotImplementedException();
        }


    }
}
