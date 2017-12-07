using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// HTTP响应实体
    /// </summary>
    public class HttpResEntity
    {
        /// <summary>
        /// 包头集合
        /// </summary>
        public WebHeaderCollection Headers { get; set; }

        /// <summary>
        /// HTML/JSON等内容
        /// </summary>
        public string Bodys { get; set; }

        /// <summary>
        /// Cookies值
        /// </summary>
        public string Cookies { get; set; }
    }
}
