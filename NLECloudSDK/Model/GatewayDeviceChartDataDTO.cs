using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// 网关设备图表数据
    /// </summary>
    public class GatewayDeviceChartDataDTO : GatewayDeviceDataDTO
    {
        /// <summary>
        /// 图表节点
        /// </summary>
        public virtual int AxisNode { get; set; }
    }
}
