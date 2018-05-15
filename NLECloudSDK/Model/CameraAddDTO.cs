using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK.Model
{
    /// <summary>
    /// 设备摄像头添加DTO
    /// </summary>
    public class CameraAddDTO : GatewayDeviceAddDTO
    {
        public virtual string HttpIp { get; set; }


        public virtual int HttpPort { get; set; }

     
        public virtual string UserName { get; set; }

      
        public virtual string Password { get; set; }
    }
}
