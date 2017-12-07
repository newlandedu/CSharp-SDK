using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// 执行状态
    /// </summary>
    public enum ResultStatus : byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 失败
        /// </summary>
        Failure = 1,

        /// <summary>
        /// 异常
        /// </summary>
        Exception = 2,

        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 3,
    }
}
