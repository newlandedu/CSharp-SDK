using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// 网关信息
    /// </summary>
    public class GatewayInfoDTO
    {
        /// <summary>
        /// 网关ID
        /// </summary>
        public virtual int GatewayID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public virtual string KindName { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public virtual string Tag { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 创建者所在学校/企业
        /// </summary>
        public virtual string CollegeName { get; set; }

        /// <summary>
        /// 公开/私有
        /// </summary>
        public virtual Boolean IsShare { get; set; }

        /// <summary>
        /// 网关座标
        /// </summary>
        public virtual string Coordinate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
    }
}
