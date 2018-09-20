using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLECloudSDK.Model;
using System;
using System.Collections.Generic;

namespace NLECloudSDK
{
    /// <summary>
    /// 云平台API
    /// </summary>
    public class NLECloudAPI
    {

        private String mApiHost = "http://api.nlecloud.com";//未指定时使用该域名
        private String mToken = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serverUrl"></param>
        public NLECloudAPI(String serverUrl)
        {
            this.mApiHost = serverUrl;
        }

        #region -- 私有方法 -- 

        /// <summary>
        /// 验证Id与Token
        /// </summary>
        /// <returns></returns>
        private Result PrimaryKeyVerify(Int32 Id, ref String txtToken)
        {
            Result result = new Result();

            if (Id == 0)
            {
                return result.SetFailure("请求SDK参数ID不可为空！");
            }

            if (String.IsNullOrEmpty(txtToken))
                txtToken = mToken;

            if (txtToken == "")
            {
                return result.SetFailure("请求SDK参数AccessToken不可为空！");
            }

            return result;
        }

        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="txtToken"></param>
        /// <returns></returns>
        private Result TokenVerify(ref String txtToken)
        {
            Result result = new Result();
            if(string.IsNullOrEmpty(txtToken))
            {
                txtToken = this.mToken;
            }
            if(txtToken == "")
            {
                return result.SetFailure("请求SDK参数AccessToken不可为空");
            }

            return result;
        }

        /// <summary>
        /// 验证设备标识与Token
        /// </summary>
        /// <returns></returns>
        private Result GatewayTagVerify(String txtGatewayTag , ref String txtToken)
        {
            Result result = new Result();

            if (txtGatewayTag.Trim() == "")
            {
                return result.SetFailure("请输入API请求的某个目的设备标识！");
            }

            if (String.IsNullOrEmpty(txtToken))
                txtToken = mToken;

            if (txtToken == "")
            {
                return result.SetFailure("请输入API请求的头部参数AccessToken值");
            }

            return result;
        }

        /// <summary>
        /// 验证设备标识、传感标识名、Token
        /// </summary>
        /// <returns></returns>
        private Result ApiTagVerify( String txtGatewayTag , String txtApiTag , ref String txtToken)
        {
            if (txtApiTag.Trim() == "")
            {
                Result result = new Result();
                return result.SetFailure("请输入API请求的某个目的传感器ApiTag！");
            }
            else
                return GatewayTagVerify(txtGatewayTag, ref txtToken);
        }


        /// <summary>
        /// 转Base64的字符串转为二进制数组对象
        /// </summary>
        /// <param name="Value"></param>
        private Object ValueConvertToByteAry(Object Value)
        {
            if (Value is String && Value.ToString().StartsWith("\"") && Value.ToString().EndsWith("\""))
            {
                return JsonConvert.DeserializeObject<Byte[]>(Value.ToString());
            }
            return Value;
        }

        #endregion

        #region -- 帐号AP -- 

        /// <summary>
        /// 用户登录（同时返回AccessToken）
        /// </summary>
        /// <param name="submitData">登录参数对象</param>
        /// <returns></returns>
        public ResultMsg<AccountLoginResultDTO> UserLogin(AccountLoginDTO submitData)
        {
            var result = new ResultMsg<AccountLoginResultDTO>();

            //难证
            if (submitData.Account.Trim() == "" || submitData.Password.Trim() == "")
            {
                return result.SetFailure("请输入登录用户名和密码！");
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com页面的 帐号API接口 中的 用户登录得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.UserLoginUrl);
             
            //2、定义该API接口返回的对象,初始化为空
            result = RequestAPIHelper.RequestServer<AccountLoginDTO, AccountLoginResultDTO>(apiPath, submitData);

            if (result.IsSuccess())
            {
                this.mToken = result.ResultObj.AccessToken;
            }
            return result;
        }

        #endregion

        #region -- 项目API --  

        /// <summary>
        /// 查询单个项目
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
        /// <returns></returns>
        public ResultMsg<ProjectInfoDTO> GetProjectInfo(Int32 projectId, String token = null)
        {
            var result = new ResultMsg<ProjectInfoDTO>();

            //验证
            var vry = PrimaryKeyVerify(projectId, ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.ProjectInfoUrl);

            //2、根据该API接口的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{projectId}", projectId.ToString());//将API地址中的{projectid}替换成真实项目ID

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.GET;
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity, ProjectInfoDTO>(apiPath, req);

            return result;
        }

        /// <summary>
        /// 模糊查询项目
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<ListPagerSet<ProjectInfoDTO>> GetProjects(ProjectFuzzyQryPagingParas query, String token = null)
        {
            var result = new ResultMsg<ListPagerSet<ProjectInfoDTO>>();

            var vry = TokenVerify(ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.ProjectsInfoUrl);

            //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            //apiPath = apiPath.Replace("{projectId}", projectId.ToString());//将API地址中的{gatewayTag}替换成真实设备标识

            //2、Get请求，拼接querystring
            apiPath += string.Format("?{0}&{1}", "PageSize=" + query.PageSize, "StartDate=" + query.StartDate, "EndDate=" + query.EndDate, "PageIndex=" + query.PageIndex);

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.GET;
            //req.Datas = JsonFormatter.Serialize(queryData);
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity, ListPagerSet<ProjectInfoDTO>>(apiPath, req);      

            return result;
        }

        /// <summary>
        /// 查询项目所有设备的传感器
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="token">请求token</param>
        /// <returns></returns>
        public ResultMsg<IEnumerable<SensorBaseInfoDTO>> GetProjectSensors(Int32 projectId, String token = null)
        {
            var result = new ResultMsg<IEnumerable<SensorBaseInfoDTO>>();

            //验证
            var vry = PrimaryKeyVerify(projectId, ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.ProjectSensorsUrl);

            //2、根据该API接口的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{projectId}", projectId.ToString());//将API地址中的{projectid}替换成真实项目ID

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.GET;
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity, IEnumerable<SensorBaseInfoDTO>>(apiPath, req);

            return result;
        }

        #endregion

        #region -- 设备API -- 

        /// <summary>
        /// 批量查询设备最新数据 
        /// </summary>
        /// <param name="devids">设备ID用逗号隔开, 限制100个设备 </param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<IEnumerable<DeviceSensorDataDTO>> GetDevicesDatas(String devids, String token = null)
        {
            var result = new ResultMsg<IEnumerable<DeviceSensorDataDTO>>();

            var vry = TokenVerify( ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.DevicesDatasUrl);

            //2、根据该API接口的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            //apiPath = apiPath.Replace("{projectId}", projectId.ToString());//将API地址中的{projectid}替换成真实项目ID
            apiPath += string.Format("?{0}", "devIds=" + devids);
            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.GET;
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity, IEnumerable<DeviceSensorDataDTO>>(apiPath, req);

            if (result.IsSuccess() && result.ResultObj != null)
            {
                foreach (DeviceSensorDataDTO p in result.ResultObj)
                {
                    if (p.Datas != null)
                    {
                        foreach (SensorDataDTO w in p.Datas)
                        {
                            w.Value = ValueConvertToByteAry(w.Value);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 批量查询设备的在线状态
        /// </summary>
        /// <param name="devids"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<IEnumerable<OnlineDataDTO>> GetDevicesStatus(String devids, String token = null)
        {
            var result = new ResultMsg<IEnumerable<OnlineDataDTO>>();

            var vry = TokenVerify(ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.DevicesStatusUrl);

            //2、根据该API接口的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            //apiPath = apiPath.Replace("{projectId}", projectId.ToString());//将API地址中的{projectid}替换成真实项目ID
            apiPath += string.Format("?{0}", "devids=" + devids);
            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.GET;
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity, IEnumerable<OnlineDataDTO>>(apiPath, req);

            return result;
        }

        /// <summary>
        /// 查询单个设备
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<DeviceInfoDTO> GetDeviceInfo(Int32 deviceId, String token = null)
        {
            var result = new ResultMsg<DeviceInfoDTO>();

            //验证
            var vry = PrimaryKeyVerify(deviceId, ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.DeviceUrl);

            //2、根据该API接口的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{deviceId}", deviceId.ToString());//将API地址中的{projectid}替换成真实项目ID

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.GET;
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity, DeviceInfoDTO>(apiPath, req);
            if (result.IsSuccess() && result.ResultObj != null && result.ResultObj.Sensors != null)
            {
                foreach(SensorBaseInfoDTO w in result.ResultObj.Sensors)
                {
                    w.Value = ValueConvertToByteAry(w.Value);
                }
            }

            return result;
        }

        /// <summary>
        /// 模糊查询设备
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<ListPagerSet<DeviceBaseInfoDTO>> GetDevices(DeviceFuzzyQryPagingParas query, String token = null)
        {
            var result = new ResultMsg<ListPagerSet<DeviceBaseInfoDTO>>();

            var vry = TokenVerify( ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.Devices);

            //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            //apiPath = apiPath.Replace("{projectId}", projectId.ToString());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath += string.Format("?{0}&{1}", "PageSize=" + query.PageSize, "StartDate=" + query.StartDate, "EndDate=" + query.EndDate, "PageIndex=" + query.PageIndex);
            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.GET;
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity, ListPagerSet<DeviceBaseInfoDTO>>(apiPath, req);

            return result;
        }

        /// <summary>
        /// 添加一个新设备
        /// </summary>
        /// <param name="device">提交数据</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<Int32> AddDevice(DeviceAddUpdateDTO device, String token = null)
        {
            var result = new ResultMsg<Int32>();

            var vry = TokenVerify(ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.Devices);

            //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            //apiPath = apiPath.Replace("{projectId}", projectId.ToString());//将API地址中的{gatewayTag}替换成真实设备标识

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Datas = JsonFormatter.Serialize(device);
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity, Int32>(apiPath, req);

            return result;       

        }

        /// <summary>
        /// 更新某个设备
        /// </summary>
        /// <param name="device">更新数据</param>
        /// <param name="deviceId">设备ID</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Result UpdateDevice(Int32 deviceId, DeviceAddUpdateDTO device, String token = null)
        {
            var result = new Result();

            //验证
            var vry = PrimaryKeyVerify(deviceId, ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.DeviceUpdateUrl);

            //2、根据该API接口的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{deviceid}", deviceId.ToString());//将API地址中的{projectid}替换成真实项目ID

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.PUT;
            req.Datas = JsonFormatter.Serialize(device);
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity>(apiPath, req);

            return result;
        }

        /// <summary>
        /// 删除某个设备
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Result DeleteDevice(Int32 deviceId, String token = null)
        {
            var result = new Result();

            //验证
            var vry = PrimaryKeyVerify(deviceId, ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.DeviceDeleteUrl);

            //2、根据该API接口的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{deviceid}", deviceId.ToString());//将API地址中的{projectid}替换成真实项目ID

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.DELETE;
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity>(apiPath, req);

            return result;
        }

        #endregion

        #region -- 设备传感器API --

        /// <summary>
        /// 查询单个传感器
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <param name="apiTag">传感标识名</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<SensorBaseInfoDTO> GetSensorInfo(Int32 deviceId, String apiTag, String token = null)
        {
            var result = new ResultMsg<SensorBaseInfoDTO>();

            //验证
            var vry = PrimaryKeyVerify(deviceId, ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.SensorOfDeviceUrl);

            //2、根据该API接口的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{deviceid}", deviceId.ToString()).Replace("{apitag}", apiTag);//将API地址中的{projectid}替换成真实项目ID

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.GET;
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity, SensorBaseInfoDTO>(apiPath, req , (json)=> 
            {
                var qry = new ResultMsg<SensorBaseInfoDTO>();
                if (String.IsNullOrEmpty(json))
                    return qry.SetFailure("数据请求错误,返回对象为空!");

                JObject jObject = JObject.Parse(json);
                if (jObject["ResultObj"].HasValues && jObject["ResultObj"].SelectToken("Groups") != null)
                {
                    switch (jObject["ResultObj"]["Groups"].ToString())
                    {
                        case "1":
                            {
                                var tmp = JsonFormatter.Deserialize<ResultMsg<SensorInfoDTO>>(json);
                                qry.ResultObj = tmp.ResultObj;
                            }
                            break;
                        case "2":
                            {
                                var tmp = JsonFormatter.Deserialize<ResultMsg<ActuatorInfoDTO>>(json);
                                qry.ResultObj = tmp.ResultObj;
                            }
                            break;
                        case "3":
                            {
                                var tmp = JsonFormatter.Deserialize<ResultMsg<CameraInfoDTO>>(json);
                                qry.ResultObj = tmp.ResultObj;
                            }
                            break;
                    }
                    return qry;
                }
                else
                    return qry.SetFailure("数据请求错误,返回对象为空!");
            });

            if (result.IsSuccess() && result.ResultObj != null)
            {
                result.ResultObj.Value = ValueConvertToByteAry(result.ResultObj.Value);
            }

            return result;
        }

        /// <summary>
        /// 模糊查询传感器
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <param name="apiTags">传感标识名</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<IEnumerable<SensorBaseInfoDTO>> GetSensors(Int32 deviceId, String apiTags = "", String token = null)
        {
            var result = new ResultMsg<IEnumerable<SensorBaseInfoDTO>>();

            //验证
            var vry = PrimaryKeyVerify(deviceId, ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.SensorsOfDeviceUrl);

            //2、根据该API接口的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{deviceid}", deviceId.ToString());//将API地址中的{deviceId}替换成真实设备ID
            apiPath += string.Format("?{0}","apitags=" + apiTags);
            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.GET;
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity, IEnumerable<SensorBaseInfoDTO>>(apiPath, req);

            if (result.IsSuccess() && result.ResultObj != null)
            {
                foreach(SensorBaseInfoDTO w in result.ResultObj)
                {
                    ValueConvertToByteAry(w.Value);
                }
            }

            return result;
        }

        /// <summary>
        /// 添加一个新的传感器
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <param name="sensor">
        ///     传感器：为SensorAddUpdate对象时
        ///     执行器：为ActuatorAddUpdate对象时
        ///     摄像头：为CameraAddUpdate对象时
        /// </param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<Int32> AddSensor<T>(Int32 deviceId , T sensor,String token = null) where T : SensorAddUpdateBase, new()
        {
            var result = new ResultMsg<int>();

            //验证
            var vry = PrimaryKeyVerify(deviceId, ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.SensorsOfDeviceUrl);

            //2、根据该API接口的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{deviceid}", deviceId.ToString());//将API地址中的{deviceId}替换成真实设备ID

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Datas = JsonFormatter.Serialize(sensor);
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity, int>(apiPath, req);

            return result;
        }

        /// <summary>
        /// 更新某个传感器
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <param name="apiTag">传感标识名</param>
        /// <param name="sensor">
        ///     传感器：为SensorAddUpdate对象时
        ///     执行器：为ActuatorAddUpdate对象时
        ///     摄像头：为CameraAddUpdate对象时
        /// </param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Result UpdateSensor<T>(Int32 deviceId, String apiTag, T sensor, String token = null) where T : SensorAddUpdateBase, new()
        {
            var result = new ResultMsg<Result>();

            //验证
            var vry = PrimaryKeyVerify(deviceId, ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.SensorOfDeviceUrl);

            //2、根据该API接口的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{deviceid}", deviceId.ToString()).Replace("{apitag}", apiTag);//将API地址中的{projectid}替换成真实项目ID

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.PUT;
            req.Datas = JsonFormatter.Serialize(sensor);
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity, Result>(apiPath, req);

            return result;
        }

        /// <summary>
        /// 删除某个传感器
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <param name="apiTag">传感标识名</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Result DeleteSensor(Int32 deviceId, String apiTag, String token = null)
        {
            var result = new Result();

            //验证
            var vry = PrimaryKeyVerify(deviceId, ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.SensorOfDeviceUrl);

            //2、根据该API接口的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{deviceid}", deviceId.ToString()).Replace("{apitag}", apiTag);//将API地址中的{projectid}替换成真实项目ID

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.DELETE;
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity, Result>(apiPath, req);

            return result;
        }

        #endregion

        #region -- 传感数据API -- 

        /// <summary>
        /// 新增传感数据
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <param name="data">传感数据</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Result AddSensorDatas(Int32 deviceId, SensorDataListAddDTO data, String token = null)
        {
            var result = new Result();

            //验证
            var vry = PrimaryKeyVerify(deviceId, ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.DatasOfSensorUrl);

            //2、根据该API接口的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{deviceid}", deviceId.ToString());//将API地址中的{projectid}替换成真实项目ID

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Datas = JsonFormatter.Serialize(data);
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity>(apiPath, req);

            return result;
        }

        /// <summary>
        /// 模糊查询传感数据
        /// </summary>
        /// <returns></returns>
        public ResultMsg<SensorDataInfoDTO> GetSensorDatas(SensorDataFuzzyQryPagingParas query,string token = null)
        {
            var result = new ResultMsg<SensorDataInfoDTO>();

            var vry = TokenVerify(ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.DatasOfSensorUrl);

            //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{deviceid}", query.DeviceID.ToString());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath += string.Format("?{0}&{1}&{2}&{3}&{4}", "Method=" + query.Method, "TimeAgo=" + query.TimeAgo, "Sort=" + query.Sort, "PageSize=" + query.PageSize, "PageIndex=" + query.PageIndex);
            apiPath += string.Format("&{0}&{1}&{2}", "ApiTags=" + query.ApiTags, "StartDate=" + query.StartDate, "EndDate=" + query.EndDate);
            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.GET;
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity,SensorDataInfoDTO>(apiPath, req);

            if (result.IsSuccess() && result.ResultObj != null && result.ResultObj.DataPoints != null)
            {
                foreach(SensorDataAddDTO p in result.ResultObj.DataPoints)
                {
                    if (p.PointDTO != null)
                    {
                        foreach (var w in p.PointDTO)
                        {
                            ValueConvertToByteAry(w.Value);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 聚合查询传感数据
        /// </summary>
        /// <returns></returns>
        public ResultMsg<SensorDataInfoDTO> GroupingSensorDatas(SensorDataJuHeQryPagingParas query, string token = null)
        {
            var result = new ResultMsg<SensorDataInfoDTO>();

            var vry = TokenVerify(ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}/grouping", mApiHost, NLECloudAPIUrl.DatasOfSensorUrl);

            //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{deviceid}", query.DeviceID.ToString());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath += string.Format("?{0}&{1}", "GroupBy=" + query.GroupBy, "Func=" + query.Func);
            apiPath += string.Format("&{0}&{1}&{2}", "ApiTags=" + query.ApiTags, "StartDate=" + query.StartDate, "EndDate=" + query.EndDate);
            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.GET;
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity, SensorDataInfoDTO>(apiPath, req);

            if (result.IsSuccess() && result.ResultObj != null && result.ResultObj.DataPoints != null)
            {
                foreach (SensorDataAddDTO p in result.ResultObj.DataPoints)
                {
                    if (p.PointDTO != null)
                    {
                        foreach (var w in p.PointDTO)
                        {
                            ValueConvertToByteAry(w.Value);
                        }
                    }
                }
            }

            return result;
        }

        #endregion

        #region -- 发送命令/控制设备API -- 

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <param name="apiTag">传感标识名</param>
        /// <param name="data">
        /// 开关类：开=1，关=0，暂停=2 
        /// 家居类：调光灯亮度=0~254，RGB灯色度=2~239，窗帘、卷闸门、幕布打开百分比=3%~100%，红外指令=1(on)2(off)
        /// 其它：integer/float/Json/String类型值
        /// </param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Result Cmds(Int32 deviceId, String apiTag, Object data, String token = null)
        {
            var result = new Result();

            var vry = PrimaryKeyVerify(deviceId , ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.CmdUrl);
            apiPath += string.Format("?{0}&{1}", "deviceId=" + deviceId, "apiTag=" + apiTag);
            //2、根据该API接口的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            //apiPath = apiPath.Replace("{deviceid}", deviceId.ToString());//将API地址中的{projectid}替换成真实项目ID

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Datas = JsonFormatter.Serialize(data);
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity>(apiPath, req);

            return result;

        }
        #endregion
    }
}
