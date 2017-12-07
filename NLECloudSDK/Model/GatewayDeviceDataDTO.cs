using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// 网关设备数据
    /// </summary>
    public class GatewayDeviceDataDTO
    {
        /// <summary>
        /// 网关设备ID
        /// </summary>
        public virtual Int32 GatewayDeviceID { get; set; }

        /// <summary>
        /// API标识
        /// </summary>
        public virtual String ApiTag { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual String Name { get; set; }

        /// <summary>
        /// 设备值
        /// </summary>
        public virtual String Value { get; set; }


    }
}
