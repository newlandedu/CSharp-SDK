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
        /// 返回的数据量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public virtual Int32 DeviceId { get; set; }

        /// <summary>
        /// 传感数据列表
        /// </summary>
        public IEnumerable<SensorDataAddDTO> DataPoints { get; set; }        
    }

    /// <summary>
    /// 分页传感数据
    /// </summary>
    public class SensorDataPageDTO : SensorDataInfoDTO
    {

        /// <summary>
        /// 页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 返回的数据总量
        /// </summary>
        public int RecordCount { get; set; }
    }
}
