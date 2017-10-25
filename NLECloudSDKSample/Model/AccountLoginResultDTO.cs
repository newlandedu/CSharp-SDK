using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDKSample
{
    /// <summary>
    /// 用户登录结果DTO
    /// </summary>
    public class AccountLoginResultDTO
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual int UserID { get; set; }


        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// EMAIL
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public virtual string Telphone { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual bool Gender { get; set; }

        /// <summary>
        /// 学校/企业ID
        /// </summary>
        public virtual int CollegeID { get; set; }

        /// <summary>
        /// 学校/企业
        /// </summary>
        public virtual string CollegeName { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public virtual string RoleName { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public virtual int RoleID { get; set; }

        /// <summary>
        /// 调用API令牌
        /// </summary>
        public String AccessToken { get; set; }

    }
}
