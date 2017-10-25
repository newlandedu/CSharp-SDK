using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDKSample
{
    /// <summary>
    /// 网关在线数据
    /// </summary>
    public class GatewayOnlineDataDTO
    {
        /// <summary>
        /// 网关ID
        /// </summary>
        public virtual Int32 GatewayID { get; set; }

        /// <summary>
        /// 在/离线状态
        /// </summary>
        public virtual Boolean IsOnline { get; set; }

        /// <summary>
        /// 最近上线IP
        /// </summary>
        public virtual String LastOnlineIP { get; set; }

        /// <summary>
        /// 最近上线所在地
        /// </summary>
        public virtual String LastOnlineRegion { get; set; }

        /// <summary>
        /// 最近上线时间
        /// </summary>
        public virtual DateTime? LastOnlineTime { get; set; }

        /// <summary>
        /// 最后离线时间
        /// </summary>
        public virtual DateTime? LastOfflineTime { get; set; }


    }
}
