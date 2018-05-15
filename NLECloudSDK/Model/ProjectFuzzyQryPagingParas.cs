using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// 项目模糊查询参数
    /// </summary>
    public class ProjectFuzzyQryPagingParas : ApiPagingQueryBase
    {
        /// <summary>
        /// 关键字（可选，从id或name字段模糊匹配查询）
        /// </summary>
        public String Keyword { get; set; }

        /// <summary>
        /// 项目标识码（可选，一个32位字符串）
        /// </summary>
        public String ProjectTag { get; set; }

        /// <summary>
        /// 联网方案 （可选，1：WIFI 2：以太网 3:蜂窝网络 4:蓝牙）
        /// </summary>
        public Byte NetWorkKind { get; set; }
    }
}
