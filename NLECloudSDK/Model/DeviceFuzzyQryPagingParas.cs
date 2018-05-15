using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/************************************************************
*CLR版本:4.0.30319.42000
*命名空间:NLECloudSDK.Model
*文件名:DeviceFuzzyQryPagingParas
*创建时间:2018/5/4 10:32:18
==============================================================
*修改人:
*修改时间:2018/5/4 10:32:18
*修改描述:

************************************************************/
namespace NLECloudSDK.Model
{
    public class DeviceFuzzyQryPagingParas
    {
        /// <summary>
        /// 关键字（可选，从id或name字段左匹配） 
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 指定设备ID（可选，如“124,34423,2345”，多个用逗号分隔，最多100个）
        /// </summary>
        public string DeviceIds { get; set; }

        /// <summary>
        /// 设备标识（可选）
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 在线状态（可选，true|false） 
        /// </summary>
        public string IsOnline { get; set; }

        /// <summary>
        /// 数据保密性（可选，true|false）
        /// </summary>
        public string IsShare { get; set; }

        /// <summary>
        /// 项目ID或纯32位字符的项目标识码（可选）
        /// </summary>
        public string ProjectKeyWord { get; set; }

        /// <summary>
        /// 指定每页要显示的数据个数，默认20，最多100 
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 起始时间（可选，包括当天，格式YYYY-MM-DD）
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 结束时间（可选，包括当天，格式YYYY-MM-DD） 
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 指定页码
        /// </summary>
        public int PageIndex { get; set; }

    }
}
