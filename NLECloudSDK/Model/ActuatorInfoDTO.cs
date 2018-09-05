using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK.Model
{
    /// <summary>
    /// 执行器信息
    /// </summary>
    public class ActuatorInfoDTO : SensorBaseInfoDTO
    {
        /// <summary>
        /// 操作类型 1：开关型 2：开关停型 3：按钮型 4：刻度型
        /// </summary>
        public virtual Byte OperType { get; set; }

        /// <summary>
        /// 操作类型的附加属性，如刻度型时：{"MaxRange" : 180, "MinRange" : 0, "Step" : 10}
        /// </summary>
        public virtual String OperTypeAttrs { get; set; }
    }
}
