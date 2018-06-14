using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK.Model
{
    /// <summary>
    /// 传感器添加编辑基类
    /// </summary>
    public abstract class SensorAddUpdateBase
    {
        /// <summary>
        /// 名称（中英文、数字或下划线的2到10个字符）
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 标识名（英文、数字与下划线，须以英文字母开头，设备内唯一）
        /// </summary>
        public String ApiTag { get; set; }

        /// <summary>
        /// 传输类型（可选，0：只上报1：上报和下发2：报警3：故障，默认0）
        /// </summary>
        public Byte TransType { get; set; }

        /// <summary>
        /// 数据类型（可选，0：整数型1：浮点型2：布尔型3：字符型4：枚举型5：二进制型，默认0）
        /// </summary>
        public Byte DataType { get; set; }

        /// <summary>
        /// 传输类型与数据类型的属性（可选，如枚举型值以半角逗号分隔：可爱，有在，装备，蜗牛）
        /// </summary>
        public String TypeAttrs { get; set; }
    }

    /// <summary>
    /// 传感器添加
    /// </summary>
    public class SensorAddUpdate:SensorAddUpdateBase
    {
        /// <summary>
        /// 单位（可选，定义传感器的单位）
        /// </summary>
        public String Unit { get; set; }

        /// <summary>
        /// 精度（可选，默认保留两位小数）
        /// </summary>
        public Byte Precision { get; set; }
    }

    /// <summary>
    /// 执行器添加
    /// </summary>
    public class ActuatorAddUpdate : SensorAddUpdateBase
    {
        /// <summary>
        /// 操作类型（1：开关型 2：开关停型 3：按钮型 4：刻度型）
        /// </summary>
        public Byte OperType { get; set; }

        /// <summary>
        /// 操作类型的附加属性（JSON格式，如刻度型时定义：{"MaxRange" : 180 ,"MinRange" : 0, "Step" : 10}）
        /// </summary>
        public String OperTypeAttrs { get; set; }

        /// <summary>
        /// 序列号（可选，同一类型的多个以此区别，默认0）
        /// </summary>
        public Int32 SerialNumber { get; set; }
    }

    /// <summary>
    /// 摄像头添加
    /// </summary>
    public class CameraAddUpdate : SensorAddUpdateBase
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public String HttpIp { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public Int32 HttpPort { get; set; }

        /// <summary>
        /// 登录用户名
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public String Password { get; set; }
    }
}
