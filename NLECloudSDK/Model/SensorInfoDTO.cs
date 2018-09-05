using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK.Model
{
    /// <summary>
    /// 传感器信息
    /// </summary>
    public class SensorInfoDTO : SensorBaseInfoDTO
    {
        /// <summary>
        /// 传感器单位
        /// </summary>
        public virtual String Unit { get; set; }

    }
}
