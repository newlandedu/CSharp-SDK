using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/************************************************************
*CLR版本:4.0.30319.42000
*命名空间:NLECloudSDK.Model
*文件名:SensorDataDTO
*创建时间:2018/5/4 8:40:53
==============================================================
*修改人:
*修改时间:2018/5/4 8:40:53
*修改描述:

************************************************************/
namespace NLECloudSDK.Model
{
    /// <summary>
    /// 传感数据
    /// </summary>
    public class SensorDataDTO : SensorDataPointDTO
    {
        /// <summary>
        /// 传感标识名（设备范围内唯一）
        /// </summary>
        public virtual String ApiTag { get; set; }
    }
}
