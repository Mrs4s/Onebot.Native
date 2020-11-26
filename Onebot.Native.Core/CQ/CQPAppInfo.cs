using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Onebot.Native.Core.Extensions;

namespace Onebot.Native.Core.CQ
{
    public class AppInfo
    {
        [JsonProperty("ret")]
        public int ResultCode { get; set; }

        [JsonProperty("apiver")]
        public int ApiVersion { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("version_id")]
        public int VersionId { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("event")]
        public List<AppEvent> Events { get; set; }

        [JsonProperty("menu")]
        public List<AppMenu> Menus { get; set; }
    }

    public enum AppEventType
    {
        [Alias("群消息")]
        GroupMessage = 2,

        [Alias("讨论组消息")]
        DiscussMessage = 4,

        [Alias("群文件上传")]
        GroupFileUpload = 11,

        [Alias("私聊消息")]
        PrivateMessage = 21,

        [Alias("群管理变更")]
        GroupManagerChange = 101,

        GroupMemberDecrease = 102,
        GroupMemberIncrease = 103,
        GroupMemberBanSpeak = 104,
        FriendAdd = 201,
        FriendAddRequest = 301,
        GroupAddRequest = 302,
        Startup = 1001,
        Exit = 1002,
        AppEnable = 1003,
        AppDisable = 1004
    }

	public class AppEvent
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public AppEventType Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("function")]
        public string Function { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }
	}

    public class AppMenu
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("function")]
        public string Function { get; set; }
    }
}
