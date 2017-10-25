using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDKSample
{
    /// <summary>
    /// 网关设备分页列表数据
    /// </summary>
    public class GatewayDeviceListDataDTO : GatewayDeviceDataDTO
    {
        /// <summary>
        /// 单位
        /// </summary>
        public virtual String Unit { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public virtual DateTime? RecordTime { get; set; }
    }
}
