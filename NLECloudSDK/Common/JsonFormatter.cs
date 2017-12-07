using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// JsonFormatter
    /// </summary>
    public sealed class JsonFormatter 
    {

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="instance">对象</param>
        /// <returns>序列化后的字符串</returns>
        public static string Serialize(object instance)
        {
            //if (instance == null) throw new ArgumentNullException("instance");
            //var str = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue }.Serialize(instance);
            //str = Regex.Replace(str, @"\\/Date\((\d+)\)\\/", match =>
            //{
            //    DateTime dt = new DateTime(1970, 1, 1);
            //    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
            //    dt = dt.ToLocalTime();
            //    return dt.ToString("yyyy-MM-dd HH:mm:ss");
            //});
            //return str;
            return Newtonsoft.Json.JsonConvert.SerializeObject(instance);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json">序列化的字符串</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static object Deserialize(string json, Type type)
        {
        //    if (type == null) throw new ArgumentNullException("types");
        //    return new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue }.Deserialize(json, type);
            return Newtonsoft.Json.JsonConvert.DeserializeObject(json, type);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }
}
