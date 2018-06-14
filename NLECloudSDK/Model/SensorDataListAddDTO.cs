using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NLECloudSDK.Model
{
    /// <summary>
    /// 传感数据添加DTO
    /// </summary>
    public class SensorDataListAddDTO
    {
        /// <summary>
        /// 传感数据列表
        /// </summary>
        public virtual IEnumerable<SensorDataAddDTO> DatasDTO { get; set; }

    }

    /// <summary>
    /// 传感数据添加DTO
    /// </summary>
    public class SensorDataAddDTO
    {
        /// <summary>
        /// 传感标识名（设备范围内唯一）
        /// </summary>
        public virtual String ApiTag { get; set; }

        /// <summary>
        /// 传感数据列表
        /// </summary>
        public virtual IEnumerable<SensorDataPointDTO> PointDTO { get; set; }
    }
}
