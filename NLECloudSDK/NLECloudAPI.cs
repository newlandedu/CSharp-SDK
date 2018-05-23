using Newtonsoft.Json;
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
        /// PrimaryKeyVerify
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
        /// GatewayTagVerify
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
        /// ApiTagVerify
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
            else
            {
                Console.WriteLine(result.Msg);
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
        /// <param name="query"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<ListPagerSet<ProjectInfoDTO>> GetProjectsInfo(ProjectFuzzyQryPagingParas queryData, String token = null)
        {
            var result = new ResultMsg<ListPagerSet<ProjectInfoDTO>>();

            #region --验证--
            //if (String.IsNullOrEmpty(txtToken))
            var vry = TokenVerify(ref token);
            if (!vry.IsSuccess())
            {
                vry.CopyTo(result);
                return result;
            }
            #endregion

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.ProjectsInfoUrl);

            //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            //apiPath = apiPath.Replace("{projectId}", projectId.ToString());//将API地址中的{gatewayTag}替换成真实设备标识

            //2、Get请求，拼接querystring
            apiPath += string.Format("?{0}&{1}","PageSize=" + queryData.PageSize,"StartDate=" + queryData.StartDate,"EndDate=" + queryData.EndDate,"PageIndex=" + queryData.PageIndex);

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
        /// <param name="projectId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<List<SensorBaseInfoDTO>> GetAllSensorsByProject(Int32 projectId,string token = null)
        {
            var result = new ResultMsg<List<SensorBaseInfoDTO>>();

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
            result = RequestAPIHelper.RequestServer<HttpReqEntity, List<SensorBaseInfoDTO>>(apiPath, req);

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
        public ResultMsg<List<DeviceSensorDataDTO>> GetDeviceNewestDatas(string devids,string token = null)
        {
            var result = new ResultMsg<List<DeviceSensorDataDTO>>();

            //验证
            //var vry = PrimaryKeyVerify(projectId, ref token);
            //if (!vry.IsSuccess())
            //{
            //    vry.CopyTo(result);
            //    return result;
            //}

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
            result = RequestAPIHelper.RequestServer<HttpReqEntity, List<DeviceSensorDataDTO>>(apiPath, req);
            
            if(result.IsSuccess() && result.ResultObj != null)
            {
                result.ResultObj.ForEach(p => 
                {
                    if (p.Datas != null)
                    {
                        p.Datas.ForEach(w =>
                        {
                            w.Value = ValueConvertToByteAry(w.Value);
                        });
                    }
                });
            }

            return result;
        }

        /// <summary>
        /// 批量查询设备的在线状态
        /// </summary>
        /// <param name="devids"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<List<OnlineDataDTO>> GetDevicesStatus(string devids,string token=null)
        {
            var result = new ResultMsg<List<OnlineDataDTO>>();

            //验证
            //var vry = PrimaryKeyVerify(projectId, ref token);
            //if (!vry.IsSuccess())
            //{
            //    vry.CopyTo(result);
            //    return result;
            //}

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.DevicesStatusUrl);

            //2、根据该API接口的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            //apiPath = apiPath.Replace("{projectId}", projectId.ToString());//将API地址中的{projectid}替换成真实项目ID
            apiPath += string.Format("?{0}","devids=" + devids);
            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.GET;
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity, List<OnlineDataDTO>>(apiPath, req);

            return result;
        }

        /// <summary>
        /// 查询单个设备
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<DeviceInfoDTO> GetDeviceByDeviceId(int deviceId,string token = null)
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
                result.ResultObj.Sensors.ForEach(w => 
                {
                    w.Value = ValueConvertToByteAry(w.Value);
                });
            }

            return result;
        }

        /// <summary>
        /// 模糊查询设备
        /// </summary>
        /// <param name="query"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<ListPagerSet<DeviceBaseInfoDTO>> GetDevices(DeviceFuzzyQryPagingParas queryData, string token = null)
        {
            var result = new ResultMsg<ListPagerSet<DeviceBaseInfoDTO>>();

            #region --ToUse--
            //if (String.IsNullOrEmpty(txtToken))
            //if (!vry.IsSuccess())
            //{
            //    vry.CopyTo(result);
            //    return result;
            //} 
            #endregion

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.Devices);

            //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            //apiPath = apiPath.Replace("{projectId}", projectId.ToString());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath += string.Format("?{0}&{1}", "PageSize=" + queryData.PageSize, "StartDate=" + queryData.StartDate, "EndDate=" + queryData.EndDate, "PageIndex=" + queryData.PageIndex);
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
        /// <param name="para"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<int> AddDevice(DeviceAddParas para,string token = null)
        {
            var result = new ResultMsg<int>();

            #region --ToUse--
            //if (String.IsNullOrEmpty(txtToken))
            //if (!vry.IsSuccess())
            //{
            //    vry.CopyTo(result);
            //    return result;
            //} 
            #endregion

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.Devices);

            //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            //apiPath = apiPath.Replace("{projectId}", projectId.ToString());//将API地址中的{gatewayTag}替换成真实设备标识

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Datas = JsonFormatter.Serialize(para);
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity, int>(apiPath, req);

            return result;       

        }

        /// <summary>
        /// 更新某个设备
        /// </summary>
        /// <param name="para"></param>
        /// <param name="deviceId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<Result> UpdateDeviceByDeviceId(DeviceAddParas para,int deviceId,string token = null)
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
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.DeviceUpdateUrl);

            //2、根据该API接口的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{deviceid}", deviceId.ToString());//将API地址中的{projectid}替换成真实项目ID

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.PUT;
            req.Datas = JsonFormatter.Serialize(para);
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity>(apiPath, req);

            return result;
        }

        /// <summary>
        /// 删除某个设备
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<Result> DeleteDeviceByDeviceId(int deviceId,string token = null)
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
        /// <param name="deviceId"></param>
        /// <param name="apiTag"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<SensorBaseInfoDTO> GetSensorOfDevice(int deviceId,string apiTag,string token = null)
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
            apiPath = apiPath.Replace("{deviceid}", deviceId.ToString()).Replace("{apitag}",apiTag) ;//将API地址中的{projectid}替换成真实项目ID

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.GET;
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity, SensorBaseInfoDTO>(apiPath, req);

            if (result.IsSuccess() && result.ResultObj != null)
            {
                result.ResultObj.Value = ValueConvertToByteAry(result.ResultObj.Value);
            }

            return result;
        }

        /// <summary>
        /// 模糊查询传感器
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<List<SensorBaseInfoDTO>> GetSensorsOfDevice(int deviceId, string token = null,string apiTags = null)
        {
            var result = new ResultMsg<List<SensorBaseInfoDTO>>();

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
            result = RequestAPIHelper.RequestServer<HttpReqEntity, List<SensorBaseInfoDTO>>(apiPath, req);

            if (result.IsSuccess() && result.ResultObj != null)
            {
                result.ResultObj.ForEach(w => 
                {
                    ValueConvertToByteAry(w.Value);
                });
                
            }

            return result;
        }

        /// <summary>
        /// 添加一个新的传感器
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<int> AddSensorOfDevice(SensorBaseQueryData sensor,int deviceId,string token = null)
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
        /// <param name="deviceId"></param>
        /// <param name="apiTag"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<Result> UpdateSensorOfDevice(SensorBaseQueryData sensor,int deviceId,string apiTag,string token = null)
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
        /// <param name="deviceId"></param>
        /// <param name="apiTag"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<Result> DeleteSensorOfDevice(int deviceId, string apiTag, string token = null)
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
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public ResultMsg<Result> AddDatasOfSensors(SensorDataListAddBaseDTO data, int deviceId,string token = null)
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
        /// 查询传感数据
        /// </summary>
        /// <returns></returns>
        public ResultMsg<SensorDataInfoDTO> GetDatasOfSensors(DatasFuzzyQryPagingParas query,string token = null)
        {
            var result = new ResultMsg<SensorDataInfoDTO>();

            #region --ToUse--
            //if (String.IsNullOrEmpty(txtToken))
            //if (!vry.IsSuccess())
            //{
            //    vry.CopyTo(result);
            //    return result;
            //} 
            #endregion

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.DatasOfSensorUrl);

            //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{deviceid}", query.deviceId.ToString());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath += string.Format("?{0}&{1}&{2}&{3}&{4}&{5}", "deviceId=" + query.deviceId, "Method=" + query.Method, "TimeAgo=" + query.TimeAgo, "Sort=" + query.Sort, "PageSize=" + query.PageSize, "PageIndex=" + query.PageIndex);
            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.GET;
            req.Headers.Add("AccessToken", token);

            //4、定义该API接口返回的对象
            result = RequestAPIHelper.RequestServer<HttpReqEntity,SensorDataInfoDTO>(apiPath, req);

            if (result.IsSuccess() && result.ResultObj != null && result.ResultObj.DataPoints != null)
            {
                result.ResultObj.DataPoints.ForEach(p =>
                {
                    if (p.PointDTO != null)
                    {
                        foreach (var w in p.PointDTO)
                        {
                            ValueConvertToByteAry(w.Value);
                        }
                    }
                });
            }

            return result;
        }
        #endregion

        #region -- 发送命令/控制设备API -- 

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="apiTag"></param>
        /// <param name="data"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultMsg<Result> CmdDeviced(int deviceId,string apiTag,object data,string token = null)
        {
            var result = new ResultMsg<Result>();

            //验证
            //var vry = PrimaryKeyVerify(deviceId, ref token);
            //if (!vry.IsSuccess())
            //{
            //    vry.CopyTo(result);
            //    return result;
            //}

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
