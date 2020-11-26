using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Onebot.Native.Exports.CQP
{
    public interface IFuncProcessor
    {
        /// <summary>
        /// 外部处理stdcall调用
        /// </summary>
        /// <param name="authCode"></param>
        /// <param name="func"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        object Process(int authCode, string func = null, params object[] @params);
    }
}
