using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// API分页查询基础模型
    /// </summary>
    public class ApiPagingQueryBase : LibPagerParameters
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ApiPagingQueryBase()
        {
            this.PageSize = 20;
        }


        /// <summary>
        /// 开始时间
        /// </summary>
        public String StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public String EndDate { get; set; }
    }


    /// <summary>
    /// 网关历史分页在/离线状态查询参数
    /// </summary>
    public class GatewayOnOfflineHistoryQryParas : ApiPagingQueryBase
    {

    }

    /// <summary>
    /// 网关设备历史分页数据查询参数
    /// </summary>
    public class GatewayDeviceHistoryPagerQryParas : ApiPagingQueryBase
    {

    }
}
