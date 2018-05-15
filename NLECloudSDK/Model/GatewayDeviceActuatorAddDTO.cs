using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK.Model
{
    /// <summary>
    /// 设备执行器添加DTO
    /// </summary>
    public class GatewayDeviceActuatorAddDTO : GatewayDeviceAddDTO
    {
        /// <summary>
        /// 操作类型（1：开关型 2：开关停型 3：按钮型 4：刻度型）
        /// </summary>
        public virtual Byte OperType { get; set; }

        /// <summary>
        /// 操作类型的附加属性（JSON格式，如刻度型时定义：{"MaxRange" : 180 ,"MinRange" : 0, "Step" : 10}）
        /// </summary>
        public virtual String OperTypeAttrs { get; set; }


        /// <summary>
        /// 序列号（可选，同一类型的多个以此区别，默认0）
        /// </summary>
        public virtual Int32 SerialNumber { get; set; }
    }
}
