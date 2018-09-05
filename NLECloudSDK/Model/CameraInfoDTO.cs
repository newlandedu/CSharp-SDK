using NLECloudSDK.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK.Model
{
    /// <summary>
    /// 摄像头信息
    /// </summary>
    public class CameraInfoDTO : SensorBaseInfoDTO
    {
        /// <summary>
        /// 摄像头IP
        /// </summary>
        public virtual String HttpIp { get; set; }

        /// <summary>
        /// 摄像头端口
        /// </summary>
        public virtual Int32 HttpPort { get; set; }

        /// <summary>
        /// 摄像头登录用户名
        /// </summary>
        public virtual String UserName { get; set; }

        /// <summary>
        /// 摄像头登录密码
        /// </summary>
        public virtual String Password { get; set; }
    }
}
