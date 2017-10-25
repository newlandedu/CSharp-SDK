using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDKSample
{
    /// <summary>
    /// 用户登录DTO
    /// </summary>
    public class AccountLoginDTO
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public virtual String Account { get; set; }

        /// <summary>
        /// 用户密码
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
