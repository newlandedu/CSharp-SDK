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
        /// 获取传感数据时的解释：
        /// 1）带引号为字符串或枚举；
        /// 2）带引号且是Base64编码的为二进制（二进制型数据转JSON是以Base64编码成带引号字符串，记着反序列化成二进制字节）；
        /// 3）无引号是整数型或浮点型；
        /// 4）true|false是布尔值。
        /// 新增传感数据时的解释：
        /// 1）对于二进制型传感数据的新增，最大字节大小为 48 Byte
        /// </summary>
        public virtual Object Value { get; set; }

        /// <summary>
        /// 值最新上传时间（格式：YYYY-MM-DD HH:mm）
        /// </summary>
        public virtual String RecordTime { get; set; }
    }
}
