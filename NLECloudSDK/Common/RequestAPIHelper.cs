using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// API请求助手
    /// </summary>
    public class RequestAPIHelper
    {
        private static Type ResultMsgT = typeof(ResultMsg);
        private static Type ResultT = typeof(Result);

        /// <summary>
        /// 服务接口请求
        /// </summary>
        /// <param name="apiPath">接口方法地址路径地址</param>
        /// <param name="method">post/get/put</param>
        /// <param name="data">请求信息</param>
        /// <returns></returns>
        public static ResultMsg<String> RequestServer(String apiPath, HttpMethod method, String data)
        {
            String url = apiPath.ToLower().StartsWith("http://") ? apiPath : String.Format("{0}/{1}", "http://", apiPath);
            ResultMsg<String> result = new ResultMsg<String>(ResultStatus.Failure,
                String.Format("请求服务失败,可能地址不对!地址:{0}", url));

            if (method == HttpMethod.POST)
                result = HttpHelper.Post(data, url);
            else
                result = HttpHelper.Http(url, data, null, method);
            return result;
        }

        /// <summary>
        /// 服务接口请求
        /// </summary>
        /// <typeparam name="RequestT"></typeparam>
        /// <typeparam name="ResponseT"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResultMsg<ResponseT> RequestServer<RequestT, ResponseT>(String apiPath, RequestT data)
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
                    Type type = typeof(ResponseT);
                    if(type == ResultMsgT || type == ResultT)
                    {
                        ResponseT tmp = JsonFormatter.Deserialize<ResponseT>(result.ResultObj);
                        if (tmp == null)
                            resultMsg.SetFailure("数据请求错误,返回对象为空!");
                        else
                        {
                            resultMsg.ResultObj = tmp;
                        }
                    }
                    else
                    {
                        ResultMsg<ResponseT> tmp = JsonFormatter.Deserialize<ResultMsg<ResponseT>>(result.ResultObj);
                        if (tmp == null)
                            resultMsg.SetFailure("数据请求错误,返回对象为空!");
                        else
                            resultMsg = tmp;
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
        /// <typeparam name="RequestT"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Result RequestServer<RequestT>(String apiPath, RequestT data)
        {
            var qry = RequestServer<RequestT, Result>(apiPath, data);
            if(qry.IsSuccess())
                return qry.ResultObj;
            else
            {
                if (qry.ResultObj == null)
                    return new Result().SetFailure(qry.Msg);
                else
                    return qry.ResultObj;
            }
                
            
        }
    }
}
