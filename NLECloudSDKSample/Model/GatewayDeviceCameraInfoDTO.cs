using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDKSample
{
    /// <summary>
    /// 网关设备摄像头信息
    /// </summary>
    public class GatewayDeviceCameraInfoDTO : GatewayDeviceInfoDTO
    {
        /// <summary>
        /// 摄像头IP
        /// </summary>
        public virtual string HttpIp { get; set; }

        /// <summary>
        /// 摄像头端口
        /// </summary>
        public virtual int HttpPort { get; set; }

        /// <summary>
        /// 摄像头登录用户名
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 摄像头登录密码
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// 设备类别ID
        /// </summary>
        public virtual Int32 DeviceTypeID { get; set; }

        /// <summary>
        /// 设备类别名称
        /// </summary>
        public virtual String DeviceTypeName { get; set; }

        /// <summary>
        /// 设备类别
        /// </summary>
        public virtual String DeviceType { get; set; }

        /// <summary>
        /// 设备类型组别
        /// </summary>
        public virtual String DeviceGroup { get; set; }


    }
}
