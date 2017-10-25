using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDKSample
{
    /// <summary>
    /// 案例可被浏览的
    /// </summary>
    public enum ProjectAccessVerifyResult : byte
    {
        /// <summary>
        /// 公开案例可浏览
        /// </summary>
        Allow = 0,

        /// <summary>
        /// 私有案例，登录后方可浏览
        /// </summary>
        LoginedAllow = 1,

        /// <summary>
        /// 私有案例，只有项目拥有者登录后方可浏览
        /// </summary>
        CreateUserLoginedAllow = 2,
    }

    /// <summary>
    /// 项目DTO
    /// </summary>
    public class ProjectInfoDTO 
    {

        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual String Name { get; set; }

        /// <summary>
        /// 项目标识
        /// </summary>
        public virtual String Tag { get; set; }

        /// <summary>
        /// 网关ID
        /// </summary>
        public virtual Int32 GatewayID { get; set; }

        /// <summary>
        /// 网关TAG
        /// </summary>
        public virtual String GatewayTag { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public virtual String Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// 访问权限
        /// </summary>
        public virtual ProjectAccessVerifyResult AccessVerifyResult { get; set; }

    }
}
