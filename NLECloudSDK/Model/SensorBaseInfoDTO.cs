using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/************************************************************
*CLR版本:4.0.30319.42000
*命名空间:NLECloudSDK.Model
*文件名:SensorBaseInfoDTO
*创建时间:2018/5/3 16:58:36
==============================================================
*修改人:
*修改时间:2018/5/3 16:58:36
*修改描述:

************************************************************/
namespace NLECloudSDK.Model
{
    /// <summary>
    /// 传感基础信息
    /// </summary>
    public class SensorBaseInfoDTO : SensorDataPointDTO
    {
        /// <summary>
        /// 传感标识名（设备范围内唯一）
        /// </summary>
        public virtual String ApiTag { get; set; }

        /// <summary>
        /// 传感组别 1：传感器 2：执行器 3：摄像头  4：LED
        /// </summary>
        public virtual Byte Groups { get; set; }

        /// <summary>
        /// 通信协议 0:Unknown 1：Modbus 2：Zigbee 3：Tcp  4：Udp
        /// </summary>
        public virtual Byte Protocol { get; set; }

        /// <summary>
        /// 传感名称
        /// </summary>
        public virtual String Name { get; set; }

        /// <summary>
        /// 创建时间（格式：YYYY-MM-DD HH:mm）
        /// </summary>
        public virtual String CreateDate { get; set; }

        /// <summary>
        /// 传输类型，0：只上报 1：上报和下发 2：报警 3：故障
        /// </summary>
        public virtual Byte TransType { get; set; }

        /// <summary>
        /// 数据类型，0：整数型 1：浮点型 2：布尔型 3：字符型 4：枚举型 5：二进制型
        /// </summary>
        public virtual Byte DataType { get; set; }

        /// <summary>
        /// 附加属性，如枚举型(各个枚举以半角逗号分隔)：可爱，有在，装备，蜗牛
        /// </summary>
        public virtual dynamic TypeAttrs { get; set; }

        /// <summary>
        /// 所属设备ID
        /// </summary>
        public virtual Int32 DeviceID { get; set; }

        /// <summary>
        /// 传感器类型（如temperature、humidity、light之类的）
        /// </summary>
        public virtual String SensorType { get; set; }
    }
}
