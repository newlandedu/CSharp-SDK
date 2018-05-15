using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// 项目信息
    /// </summary>
    public class ProjectInfoDTO
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public virtual Int32 ProjectID { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual String Name { get; set; }

        /// <summary>
        /// 所属行业
        /// </summary>
        public virtual String Industry { get; set; }

        /// <summary>
        /// 联网方案 WIFI/以太网/蜂窝网络/蓝牙
        /// </summary>
        public virtual String NetWorkKind { get; set; }

        /// <summary>
        /// 项目标识码
        /// </summary>
        public virtual String ProjectTag { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual String CreateDate { get; set; }

        /// <summary>
        /// 项目简介
        /// </summary>
        public virtual String Remark { get; set; }
    }
}
