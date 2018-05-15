using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK.Enum
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
}
