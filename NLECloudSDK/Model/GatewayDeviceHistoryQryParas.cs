using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// 网关设备历史数据查询参数
    /// </summary>
    public class GatewayDeviceHistoryQryParas
    {
        /// <summary>
        /// 查询方式（1：XX分钟内 2：XX小时内 3：XX天内 4：XX周内 5：XX月内 6：按startDate与endDate指定日期查询）
        /// </summary>
        public int Method { get; set; }

        /// <summary>
        /// 与Method配对使用表示"多少TimeAgo Method内"的数据，例：(Method=2,TimeAgo=30)表示30小时内的历史数据
        /// </summary>
        public int TimeAgo { get; set; }
    }
}
