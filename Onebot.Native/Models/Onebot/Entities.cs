using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Onebot.Native.Models.Onebot
{
    public class LoginInfo
    {
        [JsonProperty("user_id")]
        public long UserId { get; set; }

        public string Nickname { get; set; }
    }

    public class FriendInfo
    {
        public string Nickname { get; set; }

        public string Remark { get; set; }

        [JsonProperty("user_id")]
        public long Userid { get; set; }
    }

    public class GroupInfo
    {
        [JsonProperty("group_id")]
        public long GroupId { get; set; }

        [JsonProperty("group_name")]
        public string GroupName { get; set; }

        [JsonProperty("member_count")]
        public int MemberCount { get; set; }

        [JsonProperty("max_member_count")]
        public int MaxMemberCount { get; set; }
    }

    public class GroupMemberInfo
    {
        [JsonProperty("group_id")]
        public long GroupId { get; set; }

        [JsonProperty("user_id")]
        public long Userid { get; set; }

        public string Nickname { get; set; }

        [JsonProperty("card")]
        public string CardName { get; set; }

        public string Sex { get; set; }

        public int Age { get; set; }

        public string Area { get; set; }

        [JsonProperty("join_time")]
        public long JoinTime { get; set; }

        [JsonProperty("last_sent_time")]
        public long LastSentTime { get; set; }

        public string Level { get; set; }

        public string Role { get; set; }

        public string Title { get; set; }

        [JsonProperty("title_expire_time")]
        public long TitleExpireTime { get; set; }
    }

    public class GroupFileInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public long Size { get; set; }

        public int Busid { get; set; }

        public string Url { get; set; }
    }
}
