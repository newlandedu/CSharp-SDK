using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDKSample
{
    /// <summary>
    /// 网关设备传感器信息
    /// </summary>
    public class GatewayDeviceSensorInfoDTO : GatewayDeviceInfoDTO
    {
        /// <summary>
        /// 设备单位
        /// </summary>
        public virtual String Unit { get; set; }

        /// <summary>
        /// 设备类别ID
        /// </summary>
        public virtual Int32 DeviceTypeID { get; set; }

        /// <summary>
        /// 设备类别名称
        /// </summary>
        public virtual String DeviceTypeName { get; set; }

        /// <summary>
        /// 设备类别
        /// </summary>
        public virtual String DeviceType { get; set; }

        /// <summary>
        /// 设备类型组别
        /// </summary>
        public virtual String DeviceGroup { get; set; }

    }
}
