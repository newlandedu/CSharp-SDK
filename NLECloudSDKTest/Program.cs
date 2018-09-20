using NLECloudSDK;
using NLECloudSDK.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
 * 环境：
 * 1、建议使用VS2012打开，DEMO使用.NET Framework 4.5框架编写
 * 2、App.config中ServerDomain定义云平台接口的API域名，默认为新大陆物联网云平台API域名
 * API调用步骤说明:
 * 1、云平台用户登录API,用户名与密码分别是云平台注册的帐号与密码，需一致，否则登录会失败。
 *    登录后会返回一个Token，该Token为所有其它API接口调用的凭证，所以调用其它API必须要先登录然后得到这个Token，
 *    再调用其它API时带上这个Token才能正确调用其它API
 * 2、接下来每个API的调用都会在代码中加以注释说明，请参考各个方法
 * 3、每个API会在下方分别列出请求的原始JSON格式或返回的JSON格式供参考，至于每个JSON转成对象，则看个个调用的代码
*/
namespace Test
{
    class Program
    {
        //默认为新大陆物联网云平台API域名，测试环境或私有云请更改为自己的
        private static String API_HOST = ApplicationSettings.Get("ApiHost");
        private static String Token;                        //登录后返回的Token
        private static NLECloudAPI SDK = null;              //SDK的封装类

        //测试的参数变量，请改成你自己的值
        private static String account = "18965562233";      //云平台登录帐号
        private static String password = "123456";          //云平台登录密码
        private static Int32 projectId = 10315;               //云平上面的项目ID
        private static String devIds = "10367";               //批量查询设备最新数据等接口用到
        private static Int32 deviceId = 10367;                //云平上面的项目下的设备ID
        private static String apiTag = "nl_temperature";     //查询传感器的传感标识名
        private static String actuatorApiTag = "nl_fan";    //发送命令的执行器标识名


        /// <summary>
        /// 控制台程序主入口
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //实例化一个SDK的封装类
            SDK = new NLECloudAPI(API_HOST);

            #region -- 登陆测试 -- 

            //以下Account与Password修改成自己的
            AccountLoginDTO dto = new AccountLoginDTO() { Account = account, Password = password };
            dynamic qry = SDK.UserLogin(dto);
            if (qry.IsSuccess())
                Token = qry.ResultObj.AccessToken;
            Console.WriteLine("登录返回JSON:" + Environment.NewLine);
            Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);

            if (String.IsNullOrEmpty(Token))
            {
                Console.WriteLine("登录获取的Token为空，不继续执行其它接口请求，除非成功！" + Environment.NewLine);
                Console.ReadKey();
                return;
            }

            #endregion

            Console.WriteLine("请输入 Y 或 N，Y将开始测试调用API，N将关闭窗口？" + Environment.NewLine);
            if (Console.ReadKey().Key.ToString() == "Y" )
            {
                Console.WriteLine();

                //开始异步执行各个接口调用
                new Thread(new ThreadStart(() => 
                {
                    ExeccuteApiTestCall();
                    Console.ReadKey();

                })).Start();
            }
        }

        /// <summary>
        /// 执行API测试调用
        /// </summary>
        private static void ExeccuteApiTestCall()
        {
            #region -- 项目测试 -- 

            //查询单个项目。===================请修改为自己的项目ID
            dynamic qry = SDK.GetProjectInfo(projectId, Token);
            Console.WriteLine("查询单个项目返回JSON:" + Environment.NewLine);
            Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);

            //模糊查询项目。===================请修改为自己的查询条件
            var query = new ProjectFuzzyQryPagingParas()
            {
                PageIndex = 1,
                PageSize = 20,
                StartDate = "2009-01-01",
                EndDate = "2018-06-14",
            };
            qry = SDK.GetProjects(query, Token);
            Console.WriteLine("模糊查询项目返回JSON:" + Environment.NewLine);
            Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);

            //查询项目所有设备的传感器。===================请修改为自己的项目ID
            qry = SDK.GetProjectSensors(projectId, Token);
            Console.WriteLine("查询项目所有设备的传感器返回JSON:" + Environment.NewLine);
            Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);

            #endregion

            #region -- 设备测试 -- 

            //批量查询设备最新数据。===================请修改为自己的设备ID串
            qry = SDK.GetDevicesDatas(devIds, Token);
            Console.WriteLine("批量查询设备最新数据返回JSON:" + Environment.NewLine);
            Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);

            //批量查询设备的在线状态。===================请修改为自己的设备ID串
            qry = SDK.GetDevicesStatus(devIds, Token);
            Console.WriteLine("批量查询设备的在线状态返回JSON:" + Environment.NewLine);
            Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);

            //查询单个设备。===================请修改为自己的设备ID
            qry = SDK.GetDeviceInfo(deviceId, Token);
            Console.WriteLine("查询单个设备返回JSON:" + Environment.NewLine);
            Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);

            //模糊查询设备
            var query1 = new DeviceFuzzyQryPagingParas()
            {
                DeviceIds = deviceId.ToString(),
                PageIndex = 1,
                PageSize = 20,
                StartDate = "2009-01-01",
                EndDate = "2018-06-14",
            };
            qry = SDK.GetDevices(query1, Token);
            Console.WriteLine("模糊查询设备返回JSON:" + Environment.NewLine);
            Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);

            //添加个新设备
            var device = new DeviceAddUpdateDTO()
            {
                Protocol = 1,
                IsTrans = true,
                ProjectIdOrTag = projectId.ToString(),
                Name = "新添加的设备",
                Tag = "newDevice2018"
            };
            qry = SDK.AddDevice(device, Token);
            Console.WriteLine("添加个新设备返回JSON:" + Environment.NewLine);
            Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);
            if (qry.IsSuccess() && qry.ResultObj > 0)
            {
                var newDeviceId = qry.ResultObj;
                //更新某个设备
                device = new DeviceAddUpdateDTO()
                {
                    Protocol = 1,
                    IsTrans = true,
                    ProjectIdOrTag = projectId.ToString(),
                    Name = "新添加的设备(更新后)",
                    Tag = "newUpdateDevice"
                };
                qry = SDK.UpdateDevice(newDeviceId, device, Token);
                Console.WriteLine("更新某个设备返回JSON:" + Environment.NewLine);
                Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);

                //删除某个设备
                qry = SDK.DeleteDevice(newDeviceId, Token);
                Console.WriteLine("删除某个设备返回JSON:" + Environment.NewLine);
                Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);
            }

            #endregion

            #region -- 设备传感器API测试 -- 

            //查询单个传感器。===================请修改为自己的设备ID,传感标识名ApiTag
            qry = SDK.GetSensorInfo(deviceId, apiTag, Token);
            Console.WriteLine("查询单个传感器返回JSON:" + Environment.NewLine);
            Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);

            //模糊查询传感器。===================请修改为自己的设备ID串
            qry = SDK.GetSensors(deviceId, "", Token);
            Console.WriteLine("模糊查询传感器返回JSON:" + Environment.NewLine);
            Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);

            //添加一个新的传感器。===================请修改为自己的设备ID,传感标识名ApiTag
            var newApiTag = "newsensor";
            SensorAddUpdate sensor = new SensorAddUpdate()
            {
                ApiTag = newApiTag,
                DataType = 1,
                Name = "新的传感器",
                Unit = "C",
            };
            //注意：创建对象是遵循以下类别创建对象
            //传感器：为SensorAddUpdate对象
            //执行器：为ActuatorAddUpdate对象
            //摄像头：为CameraAddUpdate对象
            //这里模拟创建传感SensorAddUpdate对象
            qry = SDK.AddSensor<SensorAddUpdate>(deviceId, sensor, Token);
            Console.WriteLine("添加一个新的传感器返回JSON:" + Environment.NewLine);
            Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);
            if (qry.IsSuccess() && qry.ResultObj > 0)
            {
                //更新某个传感器。===================请修改为自己的设备ID,传感标识名ApiTag
                sensor = new SensorAddUpdate()
                {
                    Name = "新的传感器(更新后)"
                };
                qry = SDK.UpdateSensor<SensorAddUpdate>(deviceId, newApiTag, sensor, Token);
                Console.WriteLine("更新某个传感器返回JSON:" + Environment.NewLine);
                Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);

                //删除某个传感器。===================请修改为自己的设备ID,传感标识名ApiTag
                qry = SDK.DeleteSensor(deviceId, newApiTag, Token);
                Console.WriteLine("删除某个传感器返回JSON:" + Environment.NewLine);
                Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);
            }

            #endregion

            #region -- 传感数据API测试 -- 

            //新增传感数据。===================请修改为自己的设备ID,传感标识名ApiTag
            var sensorData1 = new SensorDataAddDTO() { ApiTag = apiTag };
            sensorData1.PointDTO = new List<SensorDataPointDTO>()
            {
                 new SensorDataPointDTO() { Value = 30 },
                 new SensorDataPointDTO() { Value = 55 }
            };

            var apiTag2 = "binary";
            SensorDataAddDTO sensorData2 = new SensorDataAddDTO() { ApiTag = apiTag2 };
            sensorData1.PointDTO = new List<SensorDataPointDTO>()
            {
                 new SensorDataPointDTO() { Value = System.Text.Encoding.UTF8.GetBytes("asdadasd") }
            };

            var data = new SensorDataListAddDTO();
            data.DatasDTO = new List<SensorDataAddDTO>() { sensorData1, sensorData2 };
            
            qry = SDK.AddSensorDatas(deviceId, data, Token);
            Console.WriteLine("新增传感数据返回JSON:" + Environment.NewLine);
            Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);


            //模糊查询传感数据。===================请修改为自己的设备ID,传感标识名ApiTag
            var query2 = new SensorDataFuzzyQryPagingParas()
            {
                DeviceID = deviceId,
                Method = 6,
                //TimeAgo = 30,
                //ApiTags = "m_waterPH,m_waterNTU,m_waterConduct",
                StartDate = "2018-09-13 12:06:09 ",
                Sort = "DESC",
                PageSize = 30,
                PageIndex = 1
            };
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            qry = SDK.GetSensorDatas(query2, Token);
            sw.Stop();
            var tmp = ((ResultMsg<SensorDataInfoDTO>)qry);
            if (tmp.IsSuccess() && tmp.ResultObj != null)
            {

            }
            Console.WriteLine("查询传感数据返回JSON:" + Environment.NewLine);
            Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);

            //聚合查询传感数据。===================请修改为自己的设备ID,传感标识名ApiTag
            var query3 = new SensorDataJuHeQryPagingParas()
            {
                DeviceID = deviceId,
                //ApiTags = "nl_temperature,nl_fan",
                GroupBy = 2,
                Func = "MAX",
                StartDate = "2018-01-02 12:06:09"
            };
            sw.Restart();
            qry = SDK.GroupingSensorDatas(query3, Token);
            sw.Stop();
            Console.WriteLine("聚合查询传感数据返回JSON:" + Environment.NewLine);
            Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);

            #endregion

            #region -- 命令API测试 -- 

            //发送命令。===================请修改为自己的设备ID,标识名ApiTag,控制值
            qry = SDK.Cmds(deviceId, actuatorApiTag, 1, Token);
            Console.WriteLine("发送命令返回JSON:" + Environment.NewLine);
            Console.WriteLine(SerializeToJson(qry) + Environment.NewLine);

            #endregion
        }

        /// <summary>
        /// 将对象序列化为JSON字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static String SerializeToJson(Object data)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(data);
        }
    }
}
