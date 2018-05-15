using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/************************************************************
*CLR版本:4.0.30319.42000
*命名空间:NLECloudSDK.Model
*文件名:DeviceSensorDataDTO
*创建时间:2018/5/4 8:38:14
==============================================================
*修改人:
*修改时间:2018/5/4 8:38:14
*修改描述:

************************************************************/
namespace NLECloudSDK.Model
{
    public class DeviceSensorDataDTO
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int DeviceID { get; set; }

        /// <summary>
        /// 设备名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 传感数据列表
        /// </summary>
        public List<SensorDataDTO> Datas { get; set; }
    }
}
