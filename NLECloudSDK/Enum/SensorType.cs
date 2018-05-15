using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*----------------------------------------------------------------
* 项目名称 ：NLECloudSDK.Enum
* 项目描述 ：
* 类 名 称 ：SensorType
* 类 描 述 ：
* 所在的域 ：DOTNET
* 命名空间 ：NLECloudSDK.Enum
* 机器名称 ：DOTNET 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：NLEDU_DotNet
* 创建时间 ：2018/5/14 19:06:52
* 更新时间 ：2018/5/14 19:06:52
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ NLEDU_DotNet 2018. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
namespace NLECloudSDK.Enum
{
    public enum SensorType:byte
    {
        Sensor = 0,
        Actuator = 1,
        Camera = 2
    }
}
