using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/************************************************************
*CLR版本:4.0.30319.42000
*命名空间:NLECloudSDK.Model
*文件名:DatasFuzzyQryPagingParas
*创建时间:2018/5/4 14:29:59
==============================================================
*修改人:
*修改时间:2018/5/4 14:29:59
*修改描述:

************************************************************/
namespace NLECloudSDK.Model
{
    public class SensorDataJuHeQryPagingParas
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SensorDataJuHeQryPagingParas()
        {
            GroupBy = 2;
            Func = "MAX";
            EndDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 设备ID
        /// </summary>
        public Int32 DeviceID { get; set; }

        /// <summary>
        /// 传感标识名（可选，多个用逗号分隔，最多50个）
        /// </summary>
        public String ApiTags { get; set; }

        /// <summary>
        /// 聚合方式（1：按分钟分组聚合 2：按小时分组聚合 3：按天分组聚合 4：按月分组聚合）
        /// </summary>
        public Int32 GroupBy { get; set; }

        /// <summary>
        /// 聚合函数（与GroupBy配对使用，可以是MAX：按最大值聚合 MIN：按最小值聚合 COUNT：按统计条数聚合）
        /// </summary>
        public String Func { get; set; }

        /// <summary>
        /// 起始时间（必填，格式YYYY-MM-DD HH:mm:ss）
        /// </summary>
        public String StartDate { get; set; }

        /// <summary>
        /// 结束时间（可选，为空默认取当前时间，格式YYYY-MM-DD HH:mm:ss）
        /// </summary>
        public String EndDate { get; set; }
    }
}
