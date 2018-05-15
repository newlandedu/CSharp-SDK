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
    public class DatasFuzzyQryPagingParas
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int deviceId { get; set; }

        /// <summary>
        /// 传感标识名（可选，多个用逗号分隔，最多50个）
        /// </summary>
        public string  ApiTags { get; set; }

        /// <summary>
        /// 查询方式（1：XX分钟内 2：XX小时内 3：XX天内 4：XX周内 5：XX月内 6：按startDate与endDate指定日期查询） 
        /// </summary>
        public int Method { get; set; }

        /// <summary>
        /// 与Method配对使用表示"多少TimeAgo Method内"的数据，例：(Method=2,TimeAgo=30)表示30小时内的历史数据 
        /// </summary>
        public Decimal TimeAgo { get; set; }

        /// <summary>
        /// 起始时间（可选，格式YYYY-MM-DD HH:mm:ss） 
        /// </summary>
        public String StartDate { get; set; }

        /// <summary>
        /// 结束时间（可选，格式YYYY-MM-DD HH:mm:ss）
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 时间排序方式，DESC:倒序，ASC升序 
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 指定每次要请求的数据条数，默认1000，最多3000
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 指定页码
        /// </summary>
        public int PageIndex { get; set; }
    }
}
