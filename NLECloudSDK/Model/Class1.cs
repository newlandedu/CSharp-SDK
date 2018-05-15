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
    public class SensorDataListAddDTO : SensorDataListAddBaseDTO
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public virtual Int32 DeviceId { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public virtual Int32 CreateUserID { get; set; }

        

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

    /// <summary>
    /// 传感数据点
    /// </summary>
    public class SensorDataPointDTO
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SensorDataPointDTO()
        {
            this.RecordTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 传感的最新值（有引号是字符串或枚举，无引号是整数型或浮点型，true|false是布尔值，索引数字是TypeAttrs枚举对应的索引值，其它为二进制型）
        /// </summary>
        public virtual dynamic Value { get; set; }

        /// <summary>
        /// 值最新上传时间（格式：YYYY-MM-DD HH:mm）
        /// </summary>
        public virtual String RecordTime { get; set; }
    }
}
