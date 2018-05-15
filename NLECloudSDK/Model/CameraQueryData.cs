using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*----------------------------------------------------------------
* 项目名称 ：NLECloudSDK.Model
* 项目描述 ：
* 类 名 称 ：CameraQueryData
* 类 描 述 ：
* 所在的域 ：DOTNET
* 命名空间 ：NLECloudSDK.Model
* 机器名称 ：DOTNET 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：NLEDU_DotNet
* 创建时间 ：2018/5/14 18:41:31
* 更新时间 ：2018/5/14 18:41:31
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ NLEDU_DotNet 2018. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
namespace NLECloudSDK.Model
{
    public class CameraQueryData:SensorBaseQueryData
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string HttpIp { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int HttpPort { get; set; }

        /// <summary>
        /// 登陆用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登陆密码
        /// </summary>
        public string Password { get; set; }
    }
}
