using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDKSample
{
    /// <summary>
    /// 封装HTTP请求的类，加上超时的判断机制
    /// </summary>
    public partial class HttpHelper
    {
        private const int TIMEOUT = 10;//默认超时时间(秒)

        #region -- 私有方法 --

        /// <summary>
        /// 关闭/中止连接请求
        /// </summary>
        /// <param name="req"></param>
        /// <param name="res"></param>
        private static void CloseRequest(HttpWebRequest req, HttpWebResponse res)
        {
            if (res != null)
            {
                res.Close();
                res = null;
            }
            if (req != null)
            {
                req.Abort();
                req = null;
            }
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="data">POST时的数据</param>
        /// <param name="url">地址</param>
        /// <param name="timeout">超时时间秒</param>
        /// <returns>返回HttpResEntity</returns>
        private static ResultMsg<HttpResEntity> webRequest(string url, HttpReqEntity data, int timeout)
        {
            ResultMsg<HttpResEntity> ret = new ResultMsg<HttpResEntity>();
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            if (timeout <= 0) timeout = TIMEOUT;
            TimeoutTask timeoutTask = new TimeoutTask(
                delegate
                {
                    try
                    {
                        request = (HttpWebRequest)WebRequest.Create(url);
                        request.ContentType = string.IsNullOrEmpty(data.ContentType) ? "application/json" : data.ContentType;
                        request.Method = data.Method.ToString();


                        if (data.Headers != null && data.Headers.Count > 0)
                            request.Headers.Add(data.Headers);

                        Encoding encoding = data.Encoding == null ? Encoding.UTF8 : data.Encoding;
                        if (data.Method == HttpMethod.POST)
                        {
                            if (!string.IsNullOrEmpty(data.Datas))
                            {
                                byte[] requestData = encoding.GetBytes(data.Datas);
                                request.ContentLength = requestData.Length;
                                using (Stream newStream = request.GetRequestStream())
                                {
                                    newStream.Write(requestData, 0, requestData.Length);
                                };
                            }
                            else
                                request.ContentLength = 0;
                        }

                        if (!string.IsNullOrEmpty(data.Cookies))
                        {
                            CookieContainer co = new CookieContainer();
                            string server = GetHost(url, true);
                            co.SetCookies(new Uri("http://" + server), data.Cookies);
                            request.CookieContainer = co;
                        }

                        //Thread.Sleep(31000);
                        response = (HttpWebResponse)request.GetResponse();
                        HttpResEntity resEntity = new HttpResEntity();
                        using (StreamReader receiveStream = new StreamReader(response.GetResponseStream(), encoding))
                        {
                            resEntity.Bodys = receiveStream.ReadToEnd();
                            resEntity.Cookies = response.Headers.Get("Set-Cookie");
                            resEntity.Headers = response.Headers;

                            ret.SetSuccess(resEntity);
                        };
                    }
                    catch (Exception ex)
                    {
                        ret.SetFailure(String.Format("请求出错\r请求地址：{0}\r可能原因：{1}", url, ex.Message));
                    }
                    finally
                    {
                        CloseRequest(request, response);
                    }
                },
                delegate//超时后处理事件
                {
                    CloseRequest(request, response);
                    ret.SetFailure(string.Format("请求出错\r请求地址：{0}\r可能原因：{1}", url, "连接超时了，网络不通或服务端无响应"));
                });
            if (timeoutTask.Wait(timeout * 1000))//不超时
            {
                return ret;
            }

            return ret;
        }

        #endregion

        #region -- POST --

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求的地址</param>
        /// <returns></returns>
        public static ResultMsg<string> Post(string url)
        {
            return Post(null, url);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="data">发送的数据</param>
        /// <param name="url">请求的地址</param>
        /// <returns></returns>
        public static ResultMsg<string> Post(string data, string url)
        {
            return Post(data, url, TIMEOUT);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="data">发送的数据</param>
        /// <param name="url">请求的地址</param>
        /// <param name="timeout">允许超时间，单位（秒）</param>
        /// <returns></returns>
        public static ResultMsg<string> Post(string data, string url, int timeout)
        {
            ResultMsg<string> result = new ResultMsg<string>();

            ResultMsg<HttpResEntity> resultMsg = Http(url, data, null, HttpMethod.POST, timeout);
            if (resultMsg.Status == ResultStatus.Success)
                result.ResultObj = resultMsg.ResultObj.Bodys;
            else
                result.SetFailure(resultMsg.Msg);

            return result;
        }

        #endregion

        #region -- Get --

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">请求的地址</param>
        /// <returns></returns>
        public static ResultMsg<string> Get(string url)
        {
            return Get(url, TIMEOUT);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">请求的地址</param>
        /// <param name="timeout">允许超时间，单位（秒）</param>
        /// <returns></returns>
        public static ResultMsg<string> Get(string url, int timeout)
        {
            ResultMsg<string> result = new ResultMsg<string>();

            ResultMsg<HttpResEntity> resultMsg = Http(url, null, null, HttpMethod.GET, TIMEOUT);
            if (resultMsg.Status == ResultStatus.Success)
                result.ResultObj = resultMsg.ResultObj.Bodys;
            else
                result.SetFailure(resultMsg.Msg);

            return result;
        }

        #endregion

        #region -- Request --


        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="data">请求的包实体</param>
        /// <returns></returns>
        public static ResultMsg<string> Http(string url, HttpReqEntity data)
        {
            ResultMsg<string> result = new ResultMsg<string>();

            ResultMsg<HttpResEntity> resultMsg = Http(url, data, TIMEOUT);
            if (resultMsg.Status == ResultStatus.Success)
                result.ResultObj = resultMsg.ResultObj.Bodys;
            else
                result.SetFailure(resultMsg.Msg);

            return result;
        }


        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="data">请求的包实体</param>
        /// <param name="timeout">允许超时间，单位（秒）</param>
        /// <returns></returns>
        public static ResultMsg<HttpResEntity> Http(string url, HttpReqEntity data, int timeout)
        {
            return webRequest(url, data, timeout);
        }


        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据</param>
        /// <param name="cookie">附加请求COOKIE</param>
        /// <param name="method">请求方式</param>
        /// <returns></returns>
        public static ResultMsg<string> Http(string url, string data, string cookie, HttpMethod method)
        {
            ResultMsg<string> result = new ResultMsg<string>();

            ResultMsg<HttpResEntity> resultMsg = Http(url, data, url, method, TIMEOUT);
            if (resultMsg.Status == ResultStatus.Success)
                result.ResultObj = resultMsg.ResultObj.Bodys;
            else
                result.SetFailure(resultMsg.Msg);

            return result;
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据</param>
        /// <param name="cookie">附加请求COOKIE</param>
        /// <param name="method">请求方式</param>
        /// <param name="timeout">允许超时间，单位（秒）</param>
        /// <returns></returns>
        public static ResultMsg<HttpResEntity> Http(string url, string data, string cookie, HttpMethod method, int timeout)
        {
            HttpReqEntity reqEntity = new HttpReqEntity()
            {
                Method = method,
                Datas = data,
                Cookies = cookie,
            };
            return webRequest(url, reqEntity, timeout);
        }

        #endregion


        /// <summary>
        /// 根据Url地址获取主机(可以是IP地址、域名、Localhost等)
        /// </summary>
        /// <param name="strReqUrl">请求Url</param>
        /// <param name="returnPort">是否返回端口号</param>
        /// <returns></returns>
        public static string GetHost(string strReqUrl, bool returnPort = false)
        {
            try
            {
                if (!strReqUrl.ToLower().StartsWith("http://"))
                    strReqUrl = string.Concat("http://", strReqUrl);
                Uri url = new Uri(strReqUrl, UriKind.RelativeOrAbsolute);
                if (returnPort && !url.IsDefaultPort)
                {
                    return string.Format("{0}:{1}", url.Host, url.Port.ToString());
                }
                return url.Host;
            }
            catch
            {
                return null;
            }
        }
    }
}
