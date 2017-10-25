using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDKSample
{
    /// <summary>
    /// 对ConfigurationSettings.AppSettings操作类
    /// </summary>
    public class ApplicationSettings
    {
        /// <summary>
        /// 获取web.config的配置项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }
            ConfigurationManager.RefreshSection("appSettings");
            string str = ConfigurationManager.AppSettings[key];
            if (str == null)
            {
                throw new ArgumentNullException("未找到" + key + "配置项");
            }
            return str;
        }

        /// <summary>
        /// 获取web.config的配置项
        /// </summary>
        /// <param name="containsKey">包含的关键词</param>
        /// <returns></returns>
        public static IList<String> GetList(string containsKey)
        {
            IList<String> result = new List<String>();
            if (string.IsNullOrEmpty(containsKey))
            {
                return null;
            }
            ConfigurationManager.RefreshSection("appSettings");
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                if (key.Contains(containsKey))
                {
                    result.Add(ConfigurationManager.AppSettings[key]);
                }
            }

            return result;
        }

        /// <summary>
        /// 设置web.config的配置项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Set(string key, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (!config.HasFile)
                throw new ArgumentException("程序配置文件缺失！");
            KeyValueConfigurationElement tmpKey = config.AppSettings.Settings[key];
            if (tmpKey == null)
                config.AppSettings.Settings.Add(key, value);
            else
                config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            return true;
        }
    }
}
