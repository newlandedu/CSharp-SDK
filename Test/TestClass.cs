using NLECloudSDK;
using NLECloudSDK.Enum;
using NLECloudSDK.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*----------------------------------------------------------------
* 项目名称 ：Test
* 项目描述 ：
* 类 名 称 ：TestClass
* 类 描 述 ：
* 所在的域 ：DOTNET
* 命名空间 ：Test
* 机器名称 ：DOTNET 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：NLEDU_DotNet
* 创建时间 ：2018/5/14 9:32:56
* 更新时间 ：2018/5/14 9:32:56
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ NLEDU_DotNet 2018. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
namespace Test
{
    public class TestClass
    {
        public TestClass()
        {
            SDK = new NLECloudAPI(TestClass.API_HOST);
        }

        //默认为新大陆物联网云平台API域名，测试环境或私有云请更改为自己的
        public static String API_HOST = ApplicationSettings.Get("ApiHost");
        public const string OUT_STRING = "【{0}】{1}\r\n";
        public string Token { get; set; }

        private NLECloudAPI SDK = null;


        #region ——登陆API测试——

        public bool UserLoginTest()
        {
            AccountLoginDTO dto = new AccountLoginDTO() { Account = "18518518185", Password = "aaaaaa" };
            ResultMsg<AccountLoginResultDTO> opResult = SDK.UserLogin(dto);
            if (opResult.IsSuccess())
            {
                this.Token = opResult.ResultObj.AccessToken;
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region ——项目API测试——
        /// <summary>
        /// 获取一个项目
        /// </summary>
        /// <returns></returns>
        public string GetProject()
        {
            int projectID = 6571;
            ResultMsg<ProjectInfoDTO> opResult = SDK.GetProjectInfo(projectID, this.Token);
            if(opResult.IsSuccess())
            {
                return opResult.ResultObj.ProjectTag;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 模糊查询项目
        /// </summary>
        /// <returns></returns>
        public ResultMsg<ListPagerSet<ProjectInfoDTO>> GetProjects()
        {
            ProjectFuzzyQryPagingParas queryData = new ProjectFuzzyQryPagingParas()
            {

                PageSize = 20,
                StartDate = "2009-01-01",
                EndDate = "2018-05-14",
                PageIndex = 1
            };

            ResultMsg<ListPagerSet<ProjectInfoDTO>> opResult =  SDK.GetProjectsInfo(queryData,this.Token);
            return opResult;
        }

        /// <summary>
        /// 根据项目ID获取该项目下的所有传感器
        /// </summary>
        public ResultMsg<List<SensorBaseInfoDTO>> GetSensorsByProjectId()
        {
            int projectId = 6571;
            ResultMsg<List<SensorBaseInfoDTO>> opResult = SDK.GetAllSensorsByProject(projectId,this.Token);

            return opResult;
        }

        #endregion

        #region ——设备API测试——
        /// <summary>
        /// 批量查询设备最新数据
        /// </summary>
        /// <returns></returns>
        public ResultMsg<List<DeviceSensorDataDTO>> GetSensorsData()
        {
            string devides = "6636";
            ResultMsg<List<DeviceSensorDataDTO>> opResult = SDK.GetDeviceNewestDatas(devides,this.Token);

            return opResult;
        }

        /// <summary>
        /// 批量查询设备的在线状态
        /// </summary>
        /// <returns></returns>
        public ResultMsg<List<OnlineDataDTO>> GetDeviceStatus()
        {
            string devides = "6636";
            ResultMsg<List<OnlineDataDTO>> opResult = SDK.GetDevicesStatus(devides, this.Token);

            return opResult;
        }

        /// <summary>
        /// 查询单个设备
        /// </summary>
        /// <returns></returns>
        public ResultMsg<DeviceInfoDTO> GetDeviceByDeviceId()
        {
            int devides = 6636;
            ResultMsg<DeviceInfoDTO> opResult = SDK.GetDeviceByDeviceId(devides, this.Token);

            return opResult;
        }

        /// <summary>
        /// 模糊查询设备
        /// </summary>
        /// <returns></returns>
        public ResultMsg<ListPagerSet<DeviceBaseInfoDTO>> GetDevices()
        {
            DeviceFuzzyQryPagingParas devicesQuery = new DeviceFuzzyQryPagingParas()
            {
                PageSize = 20,
                StartDate = "2009-01-01",
                EndDate = "2018-05-14",
                PageIndex = 1
            };

            ResultMsg<ListPagerSet<DeviceBaseInfoDTO>> opResult = SDK.GetDevices(devicesQuery,this.Token);

            return opResult;
        }

        /// <summary>
        /// 添加新设备
        /// </summary>
        /// <returns></returns>
        public ResultMsg<int> AddDevice()
        {
            DeviceAddParas deviceAddPara = new DeviceAddParas()
            {
                Protocol = 1,
                IsTrans = true,
                ProjectIdOrTag = "6571",
                Name = "新添加的设备",
                Tag = "newesetDevice",

            };

            ResultMsg<int> opResult = SDK.AddDevice(deviceAddPara,this.Token);
            return opResult;
        }

        /// <summary>
        /// 更新设备
        /// </summary>
        /// <returns></returns>
        public ResultMsg<Result> UpdateDevice()
        {
            DeviceAddParas deviceUpdatePara = new DeviceAddParas()
            {
                Protocol = 1,
                IsTrans = true,
                ProjectIdOrTag = "6571",//项目ID
                Name = "新添加的设备名字被更新了",
                Tag = "newesetDevice",

            };
            int deviceId = 6637;

            ResultMsg<Result> opResult = SDK.UpdateDeviceByDeviceId(deviceUpdatePara,deviceId, this.Token);
            return opResult;
        }
        /// <summary>
        /// 根据设备ID删除设备
        /// </summary>
        /// <returns></returns>
        public ResultMsg<Result> DeleteDeviceByDeviceId()
        {
            int deviceId = 6637;

            ResultMsg<Result> opResult = SDK.DeleteDeviceByDeviceId(deviceId,this.Token);

            return opResult;
        }

        #endregion

        #region ——设备传感器API测试——

        /// <summary>
        /// 查询单个传感器，有问题
        /// </summary>
        /// <returns></returns>
        public ResultMsg<SensorBaseInfoDTO> GetSensorOfDevice()
        {
            int deviceId = 6636;
            string DeviceTag = "temperatureSensor";
            ResultMsg<SensorBaseInfoDTO> opResult = SDK.GetSensorOfDevice(deviceId, DeviceTag, this.Token);
            return opResult;
        }

        /// <summary>
        /// 模糊查询传感器
        /// </summary>
        /// <returns></returns>
        public ResultMsg<List<SensorBaseInfoDTO>> GetSensorsOfDevice()
        {
            int deviceId = 6636;
            ResultMsg<List<SensorBaseInfoDTO>> opResult = SDK.GetSensorsOfDevice(deviceId, this.Token);

            return opResult;
        }

        /// <summary>
        /// 添加一个新的传感器
        /// </summary>
        /// <returns></returns>
        public ResultMsg<int> AddSensorOfDevice(SensorType type)
        {
            ResultMsg<int> opResult = null;
            int deviceId = 6636;
            SensorQueryData s = new SensorQueryData()
            {
                Name = "湿度传感器",
                ApiTag = "liquidSensor"
            };

            ActuatorQueryData a = new ActuatorQueryData() {
                Name = "风扇执行器",
                ApiTag = "fanActuator",
                OperType = 1,
                OperTypeAttrs = "{ 'MaxRange' : 180 ,'MinRange' : 0, 'Step' : 10}"
            };
            CameraQueryData c = new CameraQueryData()
            {
                Name ="大华摄像头",ApiTag="dahuaCamera1",HttpIp="192.168.14.111",HttpPort=5000,UserName="admin",Password="aaaaaa"
            };
            switch(type)
            {
                case SensorType.Sensor://如果是传感器
                    {
                        opResult = SDK.AddSensorOfDevice(s,deviceId,this.Token);
                        break;
                    }
                case SensorType.Actuator://如果是执行器
                    {
                        opResult = SDK.AddSensorOfDevice(a, deviceId, this.Token);
                        break;
                    }
                case SensorType.Camera://如果是摄像头
                    {
                        opResult = SDK.AddSensorOfDevice(c, deviceId, this.Token);
                        break;
                    }
                default:
                    {
                        break; 
                    }
                    
            }
            return opResult; 
        }

        /// <summary>
        /// 更新传感器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ResultMsg<Result> UpdateSensorOfDevice(SensorType type)
        {
            ResultMsg<Result> opResult = null;
            int deviceId = 6636;
            SensorQueryData s = new SensorQueryData()
            {
                Name = "湿度传感器更新2",
                ApiTag = "liquidSensor"
            };

            ActuatorQueryData a = new ActuatorQueryData()
            {
                Name = "风扇执行器更新",
                ApiTag = "fanActuator",
                OperType = 1,
                OperTypeAttrs = "{ 'MaxRange' : 200 ,'MinRange' : 0, 'Step' : 10}"
            };
            CameraQueryData c = new CameraQueryData()
            {
                Name = "大华摄像头更新",
                ApiTag = "dahuaCamera1",
                HttpIp = "192.168.14.111",
                HttpPort = 5000,
                UserName = "admin",
                Password = "aaaaaa"
            };
            switch (type)
            {
                case SensorType.Sensor://如果是传感器
                    {
                        opResult = SDK.UpdateSensorOfDevice(s, deviceId,s.ApiTag,this.Token);
                        break;
                    }
                case SensorType.Actuator://如果是执行器
                    {
                        opResult = SDK.UpdateSensorOfDevice(a, deviceId,a.ApiTag, this.Token);
                        break;
                    }
                case SensorType.Camera://如果是摄像头
                    {
                        opResult = SDK.UpdateSensorOfDevice(c, deviceId,c.ApiTag, this.Token);
                        break;
                    }
                default:
                    {
                        break;
                    }

            }
            return opResult;
        }

        /// <summary>
        /// 删除某个传感器
        /// </summary>
        /// <returns></returns>
        public ResultMsg<Result> DeleteSensorOfDevice()
        {
            int deviceId = 6636;
            string ApiTag = "liquidSensor";
            ResultMsg<Result> opResult = SDK.DeleteSensorOfDevice(deviceId,ApiTag,this.Token);

            return opResult;
        }
        #endregion

        #region ——传感数据API测试——
        #region ——新增传感数据——
        public ResultMsg<Result> AddDatasOfSensors()
        {
            int deviceId = 6636;

            SensorDataPointDTO sensorDataPointDTO1 = new SensorDataPointDTO()
            {
                Value = 30
            };
            SensorDataPointDTO sensorDataPointDTO2 = new SensorDataPointDTO()
            {
                Value = 55
            };

            List<SensorDataPointDTO> sensorDataPointDTOList = new List<SensorDataPointDTO>();
            sensorDataPointDTOList.Add(sensorDataPointDTO1);
            sensorDataPointDTOList.Add(sensorDataPointDTO2);

            SensorDataAddDTO sensorDataAddDTO1 = new SensorDataAddDTO();
            sensorDataAddDTO1.ApiTag = "temperatureSensor";
            sensorDataAddDTO1.PointDTO = sensorDataPointDTOList;

            List<SensorDataAddDTO> sensorDataAddDTOList = new List<SensorDataAddDTO>();
            sensorDataAddDTOList.Add(sensorDataAddDTO1);

            SensorDataListAddBaseDTO s = new SensorDataListAddBaseDTO();
            s.DatasDTO = sensorDataAddDTOList;



            ResultMsg<Result> opResult = SDK.AddDatasOfSensors(s, deviceId, this.Token);

            return opResult;
        }
        #endregion

        #region ——查询传感数据——
        public ResultMsg<SensorDataInfoDTO> GetDatasOfSensors()
        {
            int deviceid = 6636;
            DatasFuzzyQryPagingParas data = new DatasFuzzyQryPagingParas()
            {
                deviceId = 6636,
                Method = 2,
                TimeAgo = 30,
                Sort = "DESC",
                PageSize = 1000,
                PageIndex = 1
            };

            ResultMsg<SensorDataInfoDTO> opResult = SDK.GetDatasOfSensors(data, this.Token);

            return opResult;
        }
        #endregion
        #endregion

        #region ——命令API测试——

        public ResultMsg<Result> CmdDeviced()
        {
            int deviceId = 6636;
            string apiTag = "temperatureSensor";
            int data = 1;

            ResultMsg<Result> opResult = SDK.CmdDeviced(deviceId,apiTag,data,this.Token);

            return opResult;
        }
        #endregion


    }
}
