using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK.Model
{
    /// <summary>
    /// 设备传感数据
    /// </summary>
    public class SensorDataInfoDTO
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public virtual Int32 DeviceId { get; set; }

        /// <summary>
        /// 传感数据列表
        /// </summary>
        public IEnumerable<SensorDataAddDTO> DataPoints { get; set; }
    }
}
