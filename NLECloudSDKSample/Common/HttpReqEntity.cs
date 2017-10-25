using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDKSample
{
    /// <summary>
    /// HTTP请求实体
    /// </summary>
    public class HttpReqEntity
    {
        /// <summary>
        /// 请求方式
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// 请求编码
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// 请求格式
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 附包头集合
        /// </summary>
        public WebHeaderCollection Headers { get; set; }

        /// <summary>
        /// 附Cookies值
        /// </summary>
        public string Cookies { get; set; }

        /// <summary>
        /// 附包体数据
        /// </summary>
        public string Datas { get; set; }


        /// <summary>
        /// 构造函数
        /// </summary>
        public HttpReqEntity()
        {
            //Method = HttpMethod.POST;
            Headers = new WebHeaderCollection();
        }
    }
}
