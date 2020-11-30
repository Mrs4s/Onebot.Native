using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onebot.Native.IO
{
    public class Storage
    {
        /// <summary>
        /// 初始化插件存放路径
        /// </summary>
        public static bool InitAppStorage()
        {
            StringBuilder pathBuilder = new StringBuilder();
            pathBuilder.Append(Environment.CurrentDirectory);
            pathBuilder.Append("/apps");
            //检查目录是否存在，不存在则新建一个
            Directory.CreateDirectory(pathBuilder.ToString());
            return Directory.Exists(pathBuilder.ToString());
        }
    }
}
