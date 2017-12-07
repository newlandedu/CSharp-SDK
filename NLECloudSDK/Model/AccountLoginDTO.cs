using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// 用户登录DTO
    /// </summary>
    public class AccountLoginDTO
    {
        /// <summary>
        /// 帐号为云平台注册的手机号或用户名等
        /// </summary>
        public virtual String Account { get; set; }

        /// <summary>
        /// 密码为云平台注册的帐号密码
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// 记住密码
        /// </summary>
        public virtual bool IsRememberMe { get; set; }


        /// <summary>
        /// DTOToJson
        /// </summary>
        public String DTOToJson()
        {
            return JsonFormatter.Serialize(this);
        }

    }
}
