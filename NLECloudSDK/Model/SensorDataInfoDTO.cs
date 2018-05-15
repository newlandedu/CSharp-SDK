using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/************************************************************
*CLR版本:4.0.30319.42000
*命名空间:NLECloudSDK.Model
*文件名:SensorDataInfoDTO
*创建时间:2018/5/4 14:35:09
==============================================================
*修改人:
*修改时间:2018/5/4 14:35:09
*修改描述:

************************************************************/
namespace NLECloudSDK.Model
{
    public class SensorDataInfoDTO
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int DeviceId { get; set; }

        public List<SensorDataAddDTO> DataPoints { get; set; }
    }
}
