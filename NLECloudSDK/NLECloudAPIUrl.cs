using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    public class NLECloudAPIUrl
    {
        //  用户登录（同时返回AccessToken）
        public const String UserLoginUrl = "v2/account/login";
        //  获取某个设备的信息
        public const String GatewayInfoUrl = "v2/gateway/{gatewaytag}";
        //  获取某个设备的传感器列表
        public const String SensorListUrl = "v2/gateway/{gatewaytag}/sensorlist";
        //  获取某个传感器的信息
        public const String SensorInfoUrl = "v2/gateway/{gatewaytag}/sensor/{apitag}";
        //	获取某个设备的执行器列表
        public const String ActuatorListUrl = "v2/gateway/{gatewaytag}/actuatorlist";
        //	获取某个执行器的信息
        public const String ActuatorInfoUrl = "v2/gateway/{gatewaytag}/actuator/{apitag}";
        //	获取某个设备的摄像头列表
        public const String CameraListUrl = "v2/gateway/{gatewaytag}/cameralist";
        //	获取某个摄像头的信息
        public const String CameraInfoUrl = "v2/gateway/{gatewaytag}/camera/{apitag}";
        //	获取某个设备的当前在/离线状态
        public const String OnOfflineUrl = "v2/gateway/{gatewaytag}/onoffline";
        //	获取某个设备的历史分页在/离线状态
        public const String HistoryPagerOnofflineUrl = "v2/gateway/{gatewaytag}/historypageronoffline";
        //	获取某个设备的当前启/禁状态
        public const String GatewayStatusUrl = "v2/gateway/{gatewaytag}/status";
        //	获取某个设备的所有传感器、执行器最新值
        public const String NewestDatasUrl = "v2/gateway/{gatewaytag}/newestdatas";
        //	获取某个传感器的最新值
        public const String SensorNewestDatasUrl = "v2/gateway/{gatewaytag}/sensor/{apitag}/newestdata";
        //	获取某个传感器的历史数据
        public const String SensorHistoryDataUrl = "v2/gateway/{gatewaytag}/sensor/{apitag}/historydata";
        //	获取某个传感器的历史分页数据
        public const String SensorHistoryPagerDataUrl = "v2/gateway/{gatewaytag}/sensor/{apitag}/historypagerdata";
        //	获取某个执行器的最新值
        public const String ActuatorNewestDatasUrl = "v2/gateway/{gatewaytag}/actuator/{apitag}/newestdata";
        //	获取某个执行器的历史数据
        public const String ActuatorHistoryDataUrl = "v2/gateway/{gatewaytag}/actuator/{apitag}/historydata";
        //	获取某个执行器的历史分页数据
        public const String ActuatorHistoryPagerDataUrl = "v2/gateway/{gatewaytag}/actuator/{apitag}/historypagerdata";	
        //	控制某个执行器
        public const String ControlUrl = "v2/gateway/{gatewaytag}/actuator/{apitag}/control?data={data}";
        //	获取某个项目的信息
        public const String ProjectInfoUrl = "v2/project/{tag}";	
    }
}
