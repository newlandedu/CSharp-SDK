using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*----------------------------------------------------------------
* 项目名称 ：NLECloudSDK.Model
* 项目描述 ：
* 类 名 称 ：SensorBaseQueryData
* 类 描 述 ：
* 所在的域 ：DOTNET
* 命名空间 ：NLECloudSDK.Model
* 机器名称 ：DOTNET 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：NLEDU_DotNet
* 创建时间 ：2018/5/14 18:35:45
* 更新时间 ：2018/5/14 18:35:45
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ NLEDU_DotNet 2018. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
namespace NLECloudSDK.Model
{
    public class SensorBaseQueryData
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标识名
        /// </summary>
        public string ApiTag { get; set; }

        /// <summary>
        /// 传输类型
        /// </summary>
        public byte TransType { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public byte DataType { get; set; }
        /// <summary>
        /// 传输类型与数据类型的属性
        /// </summary>
        public String TypeAttrs { get; set; }
    }
}
