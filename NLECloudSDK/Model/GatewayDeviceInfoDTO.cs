using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// 网关设备信息
    /// </summary>
    public class GatewayDeviceInfoDTO
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public virtual int GatewayDeviceID { get; set; }

        /// <summary>
        /// 网关ID
        /// </summary>
        public virtual int GatewayID { get; set; }

        /// <summary>
        /// 通讯协议 1：modbus 2：zigbee 3：tcp  4：udp
        /// </summary>
        public virtual byte Protocol { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// API标识
        /// </summary>
        public virtual string ApiTag { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
    }
}
