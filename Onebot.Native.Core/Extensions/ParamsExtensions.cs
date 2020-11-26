using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Onebot.Native.Core.Extensions
{
    public static class ParamsExtensions
    {
        public static T GetAlias<T>(this object @this) => (T)@this.GetType()
            .GetMember(@this.ToString())
            .FirstOrDefault()?.GetCustomAttribute<AliasAttribute>()?.Alias;
    }

    public class AliasAttribute : Attribute
    {
        public object Alias { get; }

        public AliasAttribute(object alias)
        {
            Alias = alias;
        }
    }
}
