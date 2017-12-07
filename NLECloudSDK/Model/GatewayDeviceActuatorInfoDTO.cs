using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// 网关设备执行器信息
    /// </summary>
    public class GatewayDeviceActuatorInfoDTO : GatewayDeviceInfoDTO
    {
        /// <summary>
        /// 设备类别ID
        /// </summary>
        public virtual Int32 DeviceTypeID { get; set; }

        /// <summary>
        /// 设备类别名称
        /// </summary>
        public virtual Int32 DeviceTypeName { get; set; }

        /// <summary>
        /// 设备类别标识
        /// </summary>
        public virtual String DeviceType { get; set; }

        /// <summary>
        /// 设备类型组别
        /// </summary>
        public virtual String DeviceGroup { get; set; }

        /// <summary>
        /// 操作类型 1：开关型 2：开关停型 3：按钮型 4：刻度型
        /// </summary>
        public virtual Byte OperType { get; set; }

        /// <summary>
        /// 最小量程
        /// </summary>
        public virtual Int32? MinRange { get; set; }

        /// <summary>
        /// 最大量程
        /// </summary>
        public virtual Int32? MaxRange { get; set; }


    }
}
