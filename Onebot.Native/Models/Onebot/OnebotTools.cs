using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Onebot.Native.Models.Onebot
{
    public class OnebotTools
    {
        public static string ParseMessageToString(JToken token)
        {
            if (token.Type != JTokenType.Array)
                return token.ToString();
            var msg = string.Empty;
            foreach (var item in token)
            {
                var type = item["type"].ToString();
                var data = item["data"].ToObject<Dictionary<string, string>>();
                if (type == "text")
                {
                    msg += data["text"];
                    continue;
                }
                msg += $"[CQ:{type},{string.Join(",", data.Select(v => v.Key + "=" + v.Value))}]";
            }
            return msg;
        }
    }
}
