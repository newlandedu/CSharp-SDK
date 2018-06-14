using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    public class NLECloudAPIUrl
    {
        //=============================帐号API相关==============================/
        /// <summary>
        /// 用户登录（同时返回AccessToken）
        /// </summary>
        public const String UserLoginUrl = "users/login";


        //=============================项目API相关==============================/
        /// <summary>
        /// 查询单个项目
        /// </summary>
        public const String ProjectInfoUrl = "projects/{projectId}";

        /// <summary>
        /// 模糊查询项目
        /// </summary>
        public const String ProjectsInfoUrl = "projects";

        /// <summary>
        /// 查询项目所有设备的传感器
        /// </summary>
        public const String ProjectSensorsUrl = "projects/{projectId}/sensors";


        //=============================设备API相关==============================/
        /// <summary>
        /// 批量查询设备最新数据
        /// </summary>
        public const String DevicesDatasUrl = "devices/datas";

        /// <summary>
        /// 批量查询设备的在线状态
        /// </summary>
        public const String DevicesStatusUrl = "devices/status";

        /// <summary>
        /// 查询单个设备
        /// </summary>
        public const String DeviceUrl = "devices/{deviceId}";

        /// <summary>
        /// 模糊查询设备/添加一个新设备
        /// </summary>
        public const String Devices = "devices";

        /// <summary>
        /// 更新某个新设备
        /// </summary>
        public const String DeviceUpdateUrl = "devices/{deviceid}";

        /// <summary>
        /// 删除某个设备
        /// </summary>
        public const String DeviceDeleteUrl = "devices/{deviceid}";


        //=============================传感API相关==============================/
        /// <summary>
        /// 查询单个传感器
        /// </summary>
        public const String SensorOfDeviceUrl = "devices/{deviceid}/sensors/{apitag}";

        /// <summary>
        /// 模糊查询传感器/添加一个新的传感器
        /// </summary>
        public const String SensorsOfDeviceUrl = "devices/{deviceid}/sensors";



        //=============================传感数据API相关==============================/
        /// <summary>
        /// 新增传感数据/查询传感数据
        /// </summary>
        public const String DatasOfSensorUrl = "devices/{deviceid}/datas";



        //=============================发送命令API相关==============================/
        /// <summary>
        /// 发送命令
        /// </summary>
        public const String CmdUrl = "Cmds";
    }
}
