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
    public class SensorDataDTO
    {
        /// <summary>
        /// 传感标识名（设备范围内唯一） 
        /// </summary>
        public string ApiTag { get; set; }

        /// <summary>
        /// 传感的最新值（有引号是字符串或枚举，无引号是整数型或浮点型，true|false是布尔值，其它为二进制型） 
        /// </summary>
        public Object Value { get; set; }

        /// <summary>
        /// 值最新上传时间（格式：YYYY-MM-DD HH:mm） 
        /// </summary>
        public string RecordTime { get; set; }
    }
}
