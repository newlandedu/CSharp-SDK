using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// GatewayOnlineRecordListDTO
    /// </summary>
    public class GatewayOnlineRecordListDTO
    {
        /// <summary>
        /// 网关ID
        /// </summary>
        public virtual Int32 GatewayID { get; set; }

        /// <summary>
        /// 网关标签
        /// </summary>
        public virtual String GatewayTag { get; set; }

        /// <summary>
        /// 网关名称
        /// </summary>
        public virtual String GatewayName { get; set; }

        /// <summary>
        /// 上线时间
        /// </summary>
        public virtual DateTime? OnlineTime { get; set; }

        /// <summary>
        /// 在/离线状态
        /// </summary>
        public virtual Boolean IsOnline { get; set; }

        /// <summary>
        /// 上线IP
        /// </summary>
        public virtual String OnlineIP { get; set; }

        /// <summary>
        /// 上线区域
        /// </summary>
        public virtual String LastOnlineRegion { get; set; }

        /// <summary>
        /// 下线时间
        /// </summary>
        public virtual DateTime? OfflineTime { get; set; }

        /// <summary>
        /// 下线IP
        /// </summary>
        public virtual String OfflineIP { get; set; }

        /// <summary>
        /// 下线方式 1：正常退出 2：超时退出 3：异常退出
        /// </summary>
        public virtual Byte OfflineKind { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual String Remark { get; set; }

    }
}
