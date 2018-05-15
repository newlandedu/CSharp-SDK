using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/************************************************************
*CLR版本:4.0.30319.42000
*命名空间:NLECloudSDK.Model
*文件名:DeviceBaseInfoDTO
*创建时间:2018/5/4 9:37:55
==============================================================
*修改人:
*修改时间:2018/5/4 9:37:55
*修改描述:

************************************************************/
namespace NLECloudSDK.Model
{
    public class DeviceBaseInfoDTO
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int DeviceID { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设备标识
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public int ProjectID { get; set; }


        /// <summary>
        /// 协议类型 TCP/MQTT/HTTP 
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// 在线情况
        /// </summary>
        public Boolean IsOnline { get; set; }

        /// <summary>
        /// 最后上线IP
        /// </summary>
        public string LastOnlineIP { get; set; }

        /// <summary>
        /// 最后上线时间（格式：YYYY-MM-DD HH:mm）
        /// </summary>
        public string LastOnlineTime { get; set; }

        /// <summary>
        /// 设备座标（格式：经度,维度） 
        /// </summary>
        public string Coordinate { get; set; }

        /// <summary>
        /// 创建时间（格式：YYYY-MM-DD HH:mm） 
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// 数据保密性（私有:false，分享:true）
        /// </summary>
        public Boolean IsShare { get; set; }

        /// <summary>
        /// 数据传输状态（可上报：true，不可上报：false） 
        /// </summary>
        public Boolean IsTrans { get; set; }

    }
}
