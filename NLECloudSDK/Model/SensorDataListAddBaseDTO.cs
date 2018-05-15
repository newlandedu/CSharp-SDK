using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*----------------------------------------------------------------
* 项目名称 ：NLECloudSDK.Model
* 项目描述 ：
* 类 名 称 ：SensorDataListAddBaseDTO
* 类 描 述 ：
* 所在的域 ：DOTNET
* 命名空间 ：NLECloudSDK.Model
* 机器名称 ：DOTNET 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：NLEDU_DotNet
* 创建时间 ：2018/5/14 20:03:06
* 更新时间 ：2018/5/14 20:03:06
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ NLEDU_DotNet 2018. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
namespace NLECloudSDK.Model
{
    public class SensorDataListAddBaseDTO
    {
        /// <summary>
        /// 传感数据列表
        /// </summary>
        public virtual IEnumerable<SensorDataAddDTO> DatasDTO { get; set; }
    }
}
