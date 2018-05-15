using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK.Model
{
    /// <summary>
    /// 设备传感器添加DTO
    /// </summary>
    public class GatewayDeviceSensorAddDTO : GatewayDeviceAddDTO
    {

        /// <summary>
        /// 序列号（可选，同一类型的多个以此区别，默认0）
        /// </summary>
        public virtual Int32 SerialNumber { get; set; }


        /// <summary>
        /// 单位（可选，定义传感器的单位）
        /// </summary>
        public virtual String Unit { get; set; }

        /// <summary>
        /// 精度（可选，默认保留两位小数）
        /// </summary>
        public virtual Byte Precision { get; set; }

    }
}
