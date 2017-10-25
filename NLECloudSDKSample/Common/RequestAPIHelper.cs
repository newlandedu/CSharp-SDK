using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDKSample
{
    /// <summary>
    /// API请求助手
    /// </summary>
    public class RequestAPIHelper
    {
        //默认为新大陆物联网云平台API域名，测试环境或私有云请更改为自己的
        public static String ServerDomain = ApplicationSettings.Get("ServerDomain"); 

        /// <summary>
        /// 服务接口请求
        /// </summary>
        /// <param name="apiPath">接口方法地址路径地址</param>
        /// <param name="method">post/get/put</param>
        /// <param name="data">请求信息</param>
        /// <returns></returns>
        public static ResultMsg<String> RequestServer(String apiPath, HttpMethod method, String data)
        {
            String url = apiPath.StartsWith("http") ? apiPath : String.Format("{0}/{1}", ServerDomain, apiPath);
            ResultMsg<String> result = new ResultMsg<String>(ResultStatus.Failure,
                String.Format("请求服务失败,可能地址不对!地址:{0}", url));

            if (method == HttpMethod.POST)
                result = HttpHelper.Post(data, url);
            else
                result = HttpHelper.Get(url);
            return result;
        }

        /// <summary>
        /// 服务接口请求
        /// </summary>
        /// <typeparam name="RequestT"></typeparam>
        /// <typeparam name="ResponseT"></typeparam>
        /// <param name="apiName"></param>
        /// <param name="data"></param>
        /// <param name="responseTIsEntity">返回的ResponseT是实体</param>
        /// <returns></returns>
        public static ResultMsg<ResponseT> RequestServer<RequestT, ResponseT>(String apiPath, RequestT data, bool responseTIsEntity = true)
        {
            ResultMsg<ResponseT> resultMsg = new ResultMsg<ResponseT>();

            ResultMsg<String> result = null;
            if (data is HttpReqEntity)
            {
                var tmp = data as HttpReqEntity;
                String url = apiPath;
                result = HttpHelper.Http(url, tmp);
            }
            else
            {
                String strData = String.Empty;
                if (null != data) strData = JsonFormatter.Serialize(data);
                result = RequestServer(apiPath, HttpMethod.POST, strData);
            }
            
            if (result.Status == ResultStatus.Success)
            {
                if (!String.IsNullOrEmpty(result.ResultObj))
                {
                    //返回服务器原始对象
                    if (!responseTIsEntity)
                    {
                        ResponseT tmp = JsonFormatter.Deserialize<ResponseT>(result.ResultObj);
                        if (tmp == null)
                            resultMsg.SetFailure("数据请求错误,返回对象为空!");
                        else
                        {
                            ResultMsg<ResponseT> serverResult = tmp as ResultMsg<ResponseT>;
                            if (serverResult != null)
                                serverResult.CopyTo<ResponseT>(resultMsg);
                            else
                                resultMsg.SetFailure("解析JSON数据失败,返回对象不正确!");
                        }
                    }
                    else
                    {
                        ResultMsg<ResponseT> tmp = JsonFormatter.Deserialize<ResultMsg<ResponseT>>(result.ResultObj);
                        if (tmp == null)
                            resultMsg.SetFailure("数据请求错误,返回对象为空!");
                        else
                        {
                            resultMsg = tmp;
                            resultMsg.Msg = result.ResultObj;
                        }
                    }
                }
                else
                    resultMsg.SetFailure("数据请求错误,返回对象为空!");
            }
            else
                resultMsg.SetFailure(result.Msg);

            return resultMsg;
        }

        /// <summary>
        /// 服务接口请求
        /// </summary>
        /// <typeparam name="ResponseT"></typeparam>
        /// <param name="apiName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResultMsg<ResponseT> RequestServer<ResponseT>(String apiPath, bool responseTIsEntity = true)
        {
            return RequestServer<Nullable<int>, ResponseT>(apiPath, null, true);
        }


        /// <summary>
        /// 服务接口请求
        /// </summary>
        /// <typeparam name="RequestT"></typeparam>
        /// <typeparam name="ResponseT"></typeparam>
        /// <param name="apiName"></param>
        /// <param name="data"></param>
        /// <param name="responseTIsEntity">返回的ResponseT是实体</param>
        /// <returns></returns>
        public static ResultMsg<Result> RequestServer<RequestT>(String apiPath, RequestT data)
        {
            ResultMsg<Result> resultMsg = new ResultMsg<Result>();

            ResultMsg<String> result = null;
            if (data is HttpReqEntity)
            {
                var tmp = data as HttpReqEntity;
                String url = apiPath;
                result = HttpHelper.Http(url, tmp);
            }
            else
            {
                String strData = String.Empty;
                if (null != data) strData = JsonFormatter.Serialize(data);
                result = RequestServer(apiPath, HttpMethod.POST, strData);
            }

            if (result.Status == ResultStatus.Success)
            {
                if (!String.IsNullOrEmpty(result.ResultObj))
                {
                    Result tmp = JsonFormatter.Deserialize<Result>(result.ResultObj);
                    if (tmp == null)
                        resultMsg.SetFailure("数据请求错误,返回对象为空!");
                    else
                    {
                        resultMsg.ResultObj = tmp;
                        resultMsg.Msg = result.ResultObj;
                    }
                }
                else
                    resultMsg.SetFailure("数据请求错误,返回对象为空!");
            }
            else
                resultMsg.SetFailure(result.Msg);

            return resultMsg;
        }
    }
}
