using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/************************************************************
*CLR版本:4.0.30319.42000
*命名空间:NLECloudSDK.Model
*文件名:OnlineDataDTO
*创建时间:2018/5/4 8:58:23
==============================================================
*修改人:
*修改时间:2018/5/4 8:58:23
*修改描述:

************************************************************/
namespace NLECloudSDK.Model
{
    /// <summary>
    /// 设备在线数据
    /// </summary>
    public class OnlineDataDTO
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
        /// 在线状态（true为在线，false为离线）
        /// </summary>
        public virtual Boolean IsOnline { get; set; }

        /// <summary>
        /// 最近上线IP
        /// </summary>
        public virtual String LastOnlineIP { get; set; }

    }
}
