using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    public class NLECloudAPIUrl
    {
        #region ——账号API相关——
        //  用户登录（同时返回AccessToken）
        public const String UserLoginUrl = "users/login";
        #endregion


        #region ——项目API相关——
        //  查询单个项目
        public const String ProjectInfoUrl = "projects/{projectId}";
        //模糊查询项目
        public const String ProjectsInfoUrl = "projects";
        //查询项目所有设备的传感器
        public const String ProjectSensorsUrl = "projects/{projectId}/sensors";
        #endregion

        #region ——设备API相关——
        //批量查询设备最新数据
        public const String DevicesDatasUrl = "devices/datas";
        //批量查询设备的在线状态
        public const String DevicesStatusUrl = "devices/status";
        //查询单个设备 
        public const String DeviceUrl = "devices/{deviceId}";
        //模糊查询设备
        //添加一个新设备
        public const String Devices = "devices";
        //更新某个新设备
        public const String DeviceUpdateUrl = "devices/{deviceid}";
        //删除某个设备
        public const String DeviceDeleteUrl = "devices/{deviceid}";
        #endregion

        #region ——设备传感器API相关——
        //查询单个传感器
        public const String SensorOfDeviceUrl = "devices/{deviceid}/sensors/{apitag}";

        //模糊查询传感器
        //添加一个新的传感器
        public const String SensorsOfDeviceUrl = "devices/{deviceid}/sensors";



        #endregion

        #region ——传感数据API相关——
        //新增传感数据
        //查询传感数据
        public const String DatasOfSensorUrl = "devices/{deviceid}/datas";

        #endregion

        #region ——发送命令/控制设备API相关——
        public const String CmdUrl = "Cmds";
        #endregion


        #region ——v2API相关信息弃用——
        ////  获取某个设备的信息
        //public const String GatewayInfoUrl = "v2/gateway/{gatewaytag}";
        ////  获取某个设备的传感器列表
        //public const String SensorListUrl = "v2/gateway/{gatewaytag}/sensorlist";
        ////  获取某个传感器的信息
        //public const String SensorInfoUrl = "v2/gateway/{gatewaytag}/sensor/{apitag}";
        ////	获取某个设备的执行器列表
        //public const String ActuatorListUrl = "v2/gateway/{gatewaytag}/actuatorlist";
        ////	获取某个执行器的信息
        //public const String ActuatorInfoUrl = "v2/gateway/{gatewaytag}/actuator/{apitag}";
        ////	获取某个设备的摄像头列表
        //public const String CameraListUrl = "v2/gateway/{gatewaytag}/cameralist";
        ////	获取某个摄像头的信息
        //public const String CameraInfoUrl = "v2/gateway/{gatewaytag}/camera/{apitag}";
        ////	获取某个设备的当前在/离线状态
        //public const String OnOfflineUrl = "v2/gateway/{gatewaytag}/onoffline";
        ////	获取某个设备的历史分页在/离线状态
        //public const String HistoryPagerOnofflineUrl = "v2/gateway/{gatewaytag}/historypageronoffline";
        ////	获取某个设备的当前启/禁状态
        //public const String GatewayStatusUrl = "v2/gateway/{gatewaytag}/status";
        ////	获取某个设备的所有传感器、执行器最新值
        //public const String NewestDatasUrl = "v2/gateway/{gatewaytag}/newestdatas";
        ////	获取某个传感器的最新值
        //public const String SensorNewestDatasUrl = "v2/gateway/{gatewaytag}/sensor/{apitag}/newestdata";
        ////	获取某个传感器的历史数据
        //public const String SensorHistoryDataUrl = "v2/gateway/{gatewaytag}/sensor/{apitag}/historydata";
        ////	获取某个传感器的历史分页数据
        //public const String SensorHistoryPagerDataUrl = "v2/gateway/{gatewaytag}/sensor/{apitag}/historypagerdata";
        ////	获取某个执行器的最新值
        //public const String ActuatorNewestDatasUrl = "v2/gateway/{gatewaytag}/actuator/{apitag}/newestdata";
        ////	获取某个执行器的历史数据
        //public const String ActuatorHistoryDataUrl = "v2/gateway/{gatewaytag}/actuator/{apitag}/historydata";
        ////	获取某个执行器的历史分页数据
        //public const String ActuatorHistoryPagerDataUrl = "v2/gateway/{gatewaytag}/actuator/{apitag}/historypagerdata";
        ////	控制某个执行器
        //public const String ControlUrl = "v2/gateway/{gatewaytag}/actuator/{apitag}/control?data={data}";
        ////	获取某个项目的信息
        //public const String ProjectInfoUrl = "v2/project/{tag}"; 
        #endregion

    }
}
