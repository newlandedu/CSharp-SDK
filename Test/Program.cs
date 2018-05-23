using NLECloudSDK;
using NLECloudSDK.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TestClass tc = new TestClass();

            #region ——登陆测试——
            bool b = tc.UserLoginTest();
            if (b)
            {
                Console.WriteLine("登陆成功");
            }
            else
            {
                Console.WriteLine("登录失败");
            }
            #endregion

            #region ——项目测试——

            #region ——获取单个项目——
            //string projectTag = tc.GetProject();
            //if (!string.IsNullOrEmpty(projectTag))
            //{
            //    Console.WriteLine("项目tag={0}", projectTag);
            //}
            //else
            //{
            //    Console.WriteLine("获取项目失败!" +
            //        "");
            //} 
            #endregion

            #region ——模糊查询项目——
            //ResultMsg<ListPagerSet<ProjectInfoDTO>> opResult = tc.GetProjects();
            //if(opResult.IsSuccess())
            // {
            //     List<ProjectInfoDTO> listProjects = opResult.ResultObj.PageSet as List<ProjectInfoDTO> ;
            //     if(listProjects.Count > 0)
            //     {
            //         foreach (var project in listProjects)
            //         {
            //             Console.WriteLine("ProjectId={0}",project.ProjectID);
            //         }
            //     }
            //     else if(listProjects.Count == 0)
            //     {
            //         Console.WriteLine("项目个数为0");
            //     }
            // }
            //else
            // {
            //     Console.WriteLine(opResult.Msg);
            // }
            #endregion

            #region ——获取传感器——
            //ResultMsg<List<SensorBaseInfoDTO>> opResult = tc.GetSensorsByProjectId();
            //if (opResult.IsSuccess())
            //{
            //    List<SensorBaseInfoDTO> listSensors = opResult.ResultObj;
            //    if (listSensors.Count > 0)
            //    {
            //        foreach (var sensor in listSensors)
            //        {
            //            Console.WriteLine(sensor.Name);
            //        }
            //    }
            //    else if (listSensors.Count == 0)
            //    {
            //        Console.WriteLine("该项目下还未添加传感器");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine(opResult.Msg);
            //}
            #endregion

            #endregion

            #region ——设备测试——

            #region ——批量查询设备最新数据——
            //ResultMsg<List<DeviceSensorDataDTO>> opResult = tc.GetSensorsData();
            //if (opResult.IsSuccess())
            //{
            //    List<DeviceSensorDataDTO> sensorsDataList = opResult.ResultObj;
            //    if (sensorsDataList.Count > 0)
            //    {
            //        foreach (var sensorData in sensorsDataList)
            //        {

            //            Console.WriteLine(sensorData.Name);
            //        }
            //    }
            //    else if (sensorsDataList.Count == 0)
            //    {
            //        Console.WriteLine("无传感数据");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine(opResult.Msg);
            //} 
            #endregion

            #region ——批量查询设备的在线状态——
            //ResultMsg<List<OnlineDataDTO>> opResult = tc.GetDeviceStatus();
            //if (opResult.IsSuccess())
            //{
            //    List<OnlineDataDTO> onlineDataList = opResult.ResultObj;
            //    if (onlineDataList.Count > 0)
            //    {
            //        foreach (var onlineData in onlineDataList)
            //        {

            //            Console.WriteLine("{0}在线状态:{1}",onlineData.Name,onlineData.IsOnline);
            //        }
            //    }
            //    else if (onlineDataList.Count == 0)
            //    {
            //        Console.WriteLine("无传感设备");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine(opResult.Msg);
            //}
            #endregion

            #region ——查询单个设备——
            //ResultMsg<DeviceInfoDTO> opResult = tc.GetDeviceByDeviceId();
            //if (opResult.IsSuccess())
            //{
            //    DeviceInfoDTO device = opResult.ResultObj;

            //    Console.WriteLine("设备名称：{0}", device.Name);

            //}
            //else
            //{
            //    Console.WriteLine(opResult.Msg);
            //} 
            #endregion

            #region ——模糊查询设备——
            //ResultMsg<ListPagerSet<DeviceBaseInfoDTO>> opResult = tc.GetDevices();
            //if (opResult.IsSuccess())
            //{
            //    List<DeviceBaseInfoDTO> listDevices = opResult.ResultObj.PageSet as List<DeviceBaseInfoDTO>;
            //    if (listDevices.Count > 0)
            //    {
            //        foreach (var device in listDevices)
            //        {
            //            Console.WriteLine("DeviceName={0},在线状态:{1}", device.Name, device.IsOnline);
            //        }
            //    }
            //    else if (listDevices.Count == 0)
            //    {
            //        Console.WriteLine("设备个数为0");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine(opResult.Msg);
            //}
            #endregion

            #region ——添加设备——
            //ResultMsg<int> opResult = tc.AddDevice();
            //if (opResult.IsSuccess())
            //{
            //    Console.WriteLine("添加设备成功,新添加的设备ID{0}", opResult.ResultObj);
            //}
            //else
            //{
            //    Console.WriteLine(opResult.Msg);
            //} 
            #endregion

            #region ——更新设备——
            //ResultMsg<Result> opResult = tc.UpdateDevice();
            //if (opResult.IsSuccess())
            //{
            //    Console.WriteLine("更新设备成功,设备状态{0}", opResult.ResultObj.Status);
            //}
            //else
            //{
            //    Console.WriteLine(opResult.Msg);
            //}
            #endregion

            #region ——删除设备——
            //ResultMsg<Result> opResult = tc.DeleteDeviceByDeviceId();
            //if (opResult.IsSuccess())
            //{
            //    Console.WriteLine("删除设备成功{0}", opResult.ResultObj.Msg);
            //}
            //else
            //{
            //    Console.WriteLine(opResult.Msg);
            //}
            #endregion

            #endregion

            #region ——设备传感器API测试——

            #region ——查询单个传感器——
            //ResultMsg<SensorBaseInfoDTO> opResult = tc.GetSensorOfDevice();
            //if (opResult.IsSuccess())
            //{
            //    SensorBaseInfoDTO sensor = opResult.ResultObj;

            //    Console.WriteLine("传感器名称：{0}", sensor.Name);

            //}
            //else
            //{
            //    Console.WriteLine(opResult.Msg);
            //}
            #endregion

            #region ——模糊查询传感器——
            //ResultMsg<List<SensorBaseInfoDTO>> opResult = tc.GetSensorsOfDevice();
            //if (opResult.IsSuccess())
            //{
            //    List<SensorBaseInfoDTO> listSensors = opResult.ResultObj;
            //    if (listSensors.Count > 0)
            //    {
            //        foreach (var sensor in listSensors)
            //        {
            //            Console.WriteLine("DeviceName={0}", sensor.Name);
            //        }
            //    }
            //    else if (listSensors.Count == 0)
            //    {
            //        Console.WriteLine("传感器个数为0");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine(opResult.Msg);
            //}
            #endregion

            #region ——添加新的传感器——
            //ResultMsg<int> opResult = tc.AddSensorOfDevice(NLECloudSDK.Enum.SensorType.Camera);
            //if (opResult.IsSuccess())
            //{
            //    Console.WriteLine("添加成功,新添加的ID：{0}", opResult.ResultObj);
            //}
            //else
            //{
            //    Console.WriteLine(opResult.Msg);
            //}
            #endregion

            #region ——更新传感器——
            //ResultMsg<Result> opResult = tc.UpdateSensorOfDevice(NLECloudSDK.Enum.SensorType.Actuator);
            //if (opResult.IsSuccess())
            //{
            //    Console.WriteLine("更新成功");
            //}
            //else
            //{
            //    Console.WriteLine(opResult.Msg);
            //}
            #endregion

            #region ——删除某个传感器——
            //ResultMsg<Result> opResult = tc.DeleteSensorOfDevice();
            //if (opResult.IsSuccess())
            //{
            //    Console.WriteLine("删除成功");
            //}
            //else
            //{
            //    Console.WriteLine(opResult.Msg);
            //}
            #endregion

            #endregion

            #region ——传感数据API测试——

            #region ——新增传感数据——
            //ResultMsg<Result> opResult = tc.AddDatasOfSensors();
            //if (opResult.IsSuccess())
            //{
            //    Console.WriteLine("新增数据成功");
            //}
            //else
            //{
            //    Console.WriteLine(opResult.Msg);
            //}
            #endregion

            #region ——查询传感数据——
            //ResultMsg<SensorDataInfoDTO> opResult = tc.GetDatasOfSensors();
            //if (opResult.IsSuccess())
            //{
            //    Console.WriteLine("设备ID：{0}", opResult.ResultObj.DeviceId);
            //}
            //else
            //{
            //    Console.WriteLine(opResult.Msg);
            //}
            #endregion

            #endregion

            #region ——命令API测试——
            ResultMsg<Result> opResult = tc.CmdDeviced();
            if (opResult.IsSuccess())
            {
                Console.WriteLine("命令发送，操作成功");
            }
            else
            {
                Console.WriteLine(opResult.Msg);
            }
            #endregion

            Console.ReadKey();
        }
    }
}
