using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/************************************************************
*CLR版本:4.0.30319.42000
*命名空间:NLECloudSDK.Model
*文件名:DeviceInfoDTO
*创建时间:2018/5/4 9:08:15
==============================================================
*修改人:
*修改时间:2018/5/4 9:08:15
*修改描述:

************************************************************/
namespace NLECloudSDK.Model
{
    /// <summary>
    /// 设备基础信息
    /// </summary>
    public class DeviceBaseInfoDTO
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public virtual Int32 DeviceID { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public virtual String Name { get; set; }

        /// <summary>
        /// 设备标识
        /// </summary>
        public virtual String Tag { get; set; }

        /// <summary>
        /// 传输密钥
        /// </summary>
        public virtual String SecurityKey { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public virtual Int32 ProjectID { get; set; }

        /// <summary>
        /// 协议类型 TCP/MQTT/HTTP
        /// </summary>
        public virtual String Protocol { get; set; }

        /// <summary>
        /// 在线情况
        /// </summary>
        public Boolean IsOnline { get; set; }

        /// <summary>
        /// 最后上线IP
        /// </summary>
        public virtual String LastOnlineIP { get; set; }

        /// <summary>
        /// 最后上线时间（格式：YYYY-MM-DD HH:mm）
        /// </summary>
        public String LastOnlineTime { get; set; }

        /// <summary>
        /// 设备座标（格式：经度,维度）
        /// </summary>
        public virtual String Coordinate { get; set; }

        /// <summary>
        /// 创建时间（格式：YYYY-MM-DD HH:mm）
        /// </summary>
        public virtual String CreateDate { get; set; }

        /// <summary>
        /// 数据保密性（私有:false，分享:true）
        /// </summary>
        public virtual Boolean IsShare { get; set; }

        /// <summary>
        /// 数据传输状态（可上报：true，不可上报：false）
        /// </summary>
        public virtual Boolean IsTrans { get; set; }
    }


    /// <summary>
    /// 设备信息
    /// </summary>
    public class DeviceInfoDTO : DeviceBaseInfoDTO
    {
        /// <summary>
        /// 设备的传感器列表
        /// </summary>
        public virtual IEnumerable<SensorBaseInfoDTO> Sensors { get; set; }
    }
}
