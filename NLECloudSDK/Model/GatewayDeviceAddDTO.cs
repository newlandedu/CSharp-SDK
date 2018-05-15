using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK.Model
{
    /// <summary>
    /// 设备传感器添加DTO
    /// </summary>
    public class GatewayDeviceAddDTO 
    {
        private String mTypeAttrs;

        public virtual String Name { get; set; }

        /// <summary>
        /// 标识名（英文、数字与下划线，须以英文字母开头，设备内唯一）
        /// </summary>
        public virtual String ApiTag { get; set; }


        /// <summary>
        /// 传输类型（可选，0：只上报1：上报和下发2：报警3：故障，默认0）
        /// </summary>
        public virtual Byte TransType { get; set; }

        /// <summary>
        /// 数据类型（可选，0：整数型1：浮点型2：布尔型3：字符型4：枚举型5：二进制型，默认0）
        /// </summary>
        public virtual Byte DataType { get; set; }

        /// <summary>
        /// 传输类型与数据类型的属性（可选，如枚举型值以半角逗号分隔：可爱，有在，装备，蜗牛）
        /// </summary>
        public virtual String TypeAttrs
        {
            get
            {
                if (!String.IsNullOrEmpty(mTypeAttrs))
                {
                    mTypeAttrs = mTypeAttrs.Replace('，', ',');
                }
                return mTypeAttrs;
            }
            set
            {
                if (mTypeAttrs != value)
                    mTypeAttrs = value;
            }
        }
    }
}
