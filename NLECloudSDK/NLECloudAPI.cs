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

        #region ——私有方法——

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

        #endregion


        #region ——帐号API——

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


        #region ——项目API—— 

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


        #region ——设备API——
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


        #region ——设备传感器API——
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


        #region ——传感数据API——
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

            return result;
        }
        #endregion


        #region ——发送命令/控制设备API——
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


        #region ——原先的API，弃用——
        /*
/// <summary>
/// 获取某个网关的信息
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
/// <returns></returns>
public ResultMsg<GatewayInfoDTO> GetGatewayInfo(String gatewayTag, String token = null)
{
    var result = new ResultMsg<GatewayInfoDTO>();

    //难证
    var vry = GatewayTagVerify(gatewayTag, ref token);
    if (!vry.IsSuccess())
    {
        vry.CopyTo(result);
        return result;
    }

    //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
    String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.GatewayInfoUrl);

    //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
    apiPath = apiPath.Replace("{gatewaytag}", gatewayTag);//将API地址中的{gatewayTag}替换成真实设备标识

    //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
    HttpReqEntity req = new HttpReqEntity();
    req.Method = HttpMethod.GET;
    req.Headers.Add("AccessToken", token);

    //4、定义该API接口返回的对象
    result = RequestAPIHelper.RequestServer<HttpReqEntity, GatewayInfoDTO>(apiPath, req);

    return result;
}


/// <summary>
/// 获取设备列表
/// </summary>
/// <param name="apiPath">设备标识</param>
/// <param name="gatewayTag">设备标识</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
/// <returns></returns>
private ResultMsg<IEnumerable<T>> GetDeviceList<T>(String apiPath , String gatewayTag, String token = null)
{
    var result = new ResultMsg<IEnumerable<T>>();

    //难证
    var vry = GatewayTagVerify(gatewayTag, ref token);
    if (!vry.IsSuccess())
    {
        vry.CopyTo(result);
        return result;
    }

    //1、apiPath API接口路径，可以从http://api.nlecloud.com/页面的得知
    apiPath = String.Format("{0}/{1}", mApiHost, apiPath);

    //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
    apiPath = apiPath.Replace("{gatewaytag}", gatewayTag);//将API地址中的{gatewayTag}替换成真实设备标识

    //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
    HttpReqEntity req = new HttpReqEntity();
    req.Method = HttpMethod.GET;
    req.Headers.Add("AccessToken", token);

    //4、定义该API接口返回的对象,由于是列表所以要用IEnumerable
    result = RequestAPIHelper.RequestServer<HttpReqEntity, IEnumerable<T>>(apiPath, req);

    return result;
}

/// <summary>
/// 获取某个设备的信息
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="apiTag">设备标识</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
/// <returns></returns>
private ResultMsg<T> GetDeviceInfo<T>(String apiPath , String gatewayTag, String apiTag, String token = null)
{
    var result = new ResultMsg<T>();

    //难证
    var vry = ApiTagVerify(gatewayTag, apiTag, ref token);
    if (!vry.IsSuccess())
    {
        vry.CopyTo(result);
        return result;
    }

    //1、apiPath API接口路径，可以从http://api.nlecloud.com/页面的得知
    apiPath = String.Format("{0}/{1}", mApiHost, apiPath);

    //2、根据该API接口 的请求参数中 得知需要创建两个URI Parameters String类型参数，所以该参数直接跟在apiPath中
    apiPath = apiPath.Replace("{gatewaytag}", gatewayTag.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
    apiPath = apiPath.Replace("{apitag}", apiTag.Trim());//将API地址中的{apiTag}替换成真实传感器API标识

    //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
    HttpReqEntity req = new HttpReqEntity();
    req.Method = HttpMethod.GET;
    req.Headers.Add("AccessToken", token);

    //4、定义该API接口返回的对象
    result = RequestAPIHelper.RequestServer<HttpReqEntity, T>(apiPath, req);

    return result;
}

/// <summary>
/// 获取某个网关的传感器列表
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
/// <returns></returns>
public ResultMsg<IEnumerable<GatewayDeviceSensorInfoDTO>> GetSensorList(String gatewayTag, String token = null)
{
    return GetDeviceList<GatewayDeviceSensorInfoDTO>(NLECloudAPIUrl.SensorListUrl, gatewayTag, token);
}

/// <summary>
/// 获取某个传感器的信息
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="apiTag">传感器标识</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
/// <returns></returns>
public ResultMsg<GatewayDeviceSensorInfoDTO> GetSensorInfo(String gatewayTag, String apiTag, String token = null)
{
    return GetDeviceInfo<GatewayDeviceSensorInfoDTO>(NLECloudAPIUrl.SensorInfoUrl, gatewayTag, apiTag, token);
}


/// <summary>
/// 获取某个网关的执行器列表
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
/// <returns></returns>
public ResultMsg<IEnumerable<GatewayDeviceActuatorInfoDTO>> GetActuatorList(String gatewayTag, String token = null)
{
    return GetDeviceList<GatewayDeviceActuatorInfoDTO>(NLECloudAPIUrl.ActuatorListUrl, gatewayTag, token);
}

/// <summary>
/// 获取某个执行器的信息
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="apiTag">执行器标识</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
/// <returns></returns>
public ResultMsg<GatewayDeviceActuatorInfoDTO> GetActuatorInfo(String gatewayTag, String apiTag, String token = null)
{
    return GetDeviceInfo<GatewayDeviceActuatorInfoDTO>(NLECloudAPIUrl.ActuatorInfoUrl, gatewayTag, apiTag, token);
}


/// <summary>
/// 获取某个网关的摄像头列表
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
/// <returns></returns>
public ResultMsg<IEnumerable<GatewayDeviceCameraInfoDTO>> GetCameraList(String gatewayTag, String token = null)
{
    return GetDeviceList<GatewayDeviceCameraInfoDTO>(NLECloudAPIUrl.CameraListUrl, gatewayTag, token);
}

/// <summary>
/// 获取某个摄像头的信息
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="apiTag">摄像头标识</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
/// <returns></returns>
public ResultMsg<GatewayDeviceCameraInfoDTO> GetCameraInfo(String gatewayTag, String apiTag, String token = null)
{
    return GetDeviceInfo<GatewayDeviceCameraInfoDTO>(NLECloudAPIUrl.CameraInfoUrl, gatewayTag, apiTag, token);
}




/// <summary>
/// 获取某个网关的当前在/离线状态
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
/// <returns></returns>
public ResultMsg<GatewayOnlineDataDTO> GetGatewayOnOffLine(String gatewayTag, String token = null)
{
    var result = new ResultMsg<GatewayOnlineDataDTO>();


    //验证
    var vry = GatewayTagVerify(gatewayTag, ref token);
    if (!vry.IsSuccess())
    {
        vry.CopyTo(result);
        return result;
    }

    //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
    String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.OnOfflineUrl);

    //2、根据该API接口 的请求参数中 得知需要创建两个URI Parameters String类型参数，所以该参数直接跟在apiPath中
    apiPath = apiPath.Replace("{gatewaytag}", gatewayTag);//将API地址中的{gatewayTag}替换成真实设备标识

    //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
    HttpReqEntity req = new HttpReqEntity();
    req.Method = HttpMethod.GET;
    req.Headers.Add("AccessToken", token);

    //4、定义该API接口返回的对象
    result  = RequestAPIHelper.RequestServer<HttpReqEntity, GatewayOnlineDataDTO>(apiPath, req);

    return result;
}

/// <summary>
/// 获取某个网关的历史分页在/离线状态
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="query">查询分页参数</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
/// <returns></returns>
public ResultMsg<ListPagerSet<GatewayOnlineRecordListDTO>> GetHistoryPagerOnOffline(String gatewayTag, GatewayOnOfflineHistoryQryParas query, String token = null)
{
    var result = new ResultMsg<ListPagerSet<GatewayOnlineRecordListDTO>>();

    //验证
    var vry = GatewayTagVerify(gatewayTag, ref token);
    if (!vry.IsSuccess())
    {
        vry.CopyTo(result);
        return result;
    }

    //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
    String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.HistoryPagerOnofflineUrl);

    //2、根据该API接口 的请求参数中 得知需要创建一个URI Parameters String类型参数，所以该参数直接跟在apiPath中
    apiPath = apiPath.Replace("{gatewaytag}", gatewayTag);//将API地址中的{gatewayTag}替换成真实设备标识
    //2、同时再自行定义这四个URI Parameters String类型参数：StartDate、EndDate、PageIndex、PageSize
    apiPath += String.Format("?{0}&{1}&{2}&{3}"
        , "PageIndex=" + query.PageIndex                    //这里自定义当前要查询的页码数
        , "PageSize=" + query.PageSize                      //这里定义每页要查询的数量
        , "StartDate=" + query.StartDate                   //这里定义从某个时间段开始查询，现在定义为30天前
        , "EndDate=" + query.EndDate);                     //这里定义查询到某个时间点结束，现在定义为当前时间


    //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
    HttpReqEntity req = new HttpReqEntity();
    req.Method = HttpMethod.GET;
    req.Headers.Add("AccessToken", token);

    //这里跟之前设备TAB相关的接口不一样区别是，因为该接口要传递Body Parameters，所以把reqBody当包体数据赋给req

    //4、定义该API接口返回的对象
    result = RequestAPIHelper.RequestServer<HttpReqEntity, ListPagerSet<GatewayOnlineRecordListDTO>>(apiPath, req);

    return result;
}


/// <summary>
/// 获取某个网关的当前启/禁状态
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
/// <returns></returns>
public ResultMsg<Boolean> GetGatewayStatus(String gatewayTag,  String token = null)
{
    var result = new ResultMsg<Boolean>();

    //验证
    var vry = GatewayTagVerify(gatewayTag, ref token);
    if (!vry.IsSuccess())
    {
        vry.CopyTo(result);
        return result;
    }

    //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
    String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.GatewayStatusUrl);

    //2、根据该API接口 的请求参数中 得知需要创建两个URI Parameters String类型参数，所以该参数直接跟在apiPath中
    apiPath = apiPath.Replace("{gatewaytag}", gatewayTag);//将API地址中的{gatewayTag}替换成真实设备标识

    //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
    HttpReqEntity req = new HttpReqEntity();
    req.Method = HttpMethod.GET;
    req.Headers.Add("AccessToken", token);

    //4、定义该API接口返回的对象
    result = RequestAPIHelper.RequestServer<HttpReqEntity, Boolean>(apiPath, req);

    return result;
}


/// <summary>
/// 获取某个网关的所有传感器、执行器最新值
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
/// <returns></returns>
public ResultMsg<IEnumerable<GatewayDeviceDataDTO>> GetNewestDatas(String gatewayTag, String token = null)
{
    var result = new ResultMsg<IEnumerable<GatewayDeviceDataDTO>>();

    //验证
    var vry = GatewayTagVerify(gatewayTag, ref token);
    if (!vry.IsSuccess())
    {
        vry.CopyTo(result);
        return result;
    }

    //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
    String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.NewestDatasUrl);

    //2、根据该API接口 的请求参数中 得知需要创建两个URI Parameters String类型参数，所以该参数直接跟在apiPath中
    apiPath = apiPath.Replace("{gatewaytag}", gatewayTag);//将API地址中的{gatewayTag}替换成真实设备标识

    //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
    HttpReqEntity req = new HttpReqEntity();
    req.Method = HttpMethod.GET;
    req.Headers.Add("AccessToken", token);

    //4、定义该API接口返回的对象
    result = RequestAPIHelper.RequestServer<HttpReqEntity, IEnumerable<GatewayDeviceDataDTO>>(apiPath, req);

    return result;
}


/// <summary>
/// 获取某个传感器的最新值
/// </summary>
/// <param name="gatewayTag"></param>
/// <param name="apiTag"></param>
/// <param name="token"></param>
private ResultMsg<GatewayDeviceDataDTO> GetDeviceNewestData(String apiPath, String gatewayTag, String apiTag, String token = null)
{
    var result = new ResultMsg<GatewayDeviceDataDTO>();

    //验证
    var vry = ApiTagVerify(gatewayTag, apiTag, ref token);
    if (!vry.IsSuccess())
    {
        vry.CopyTo(result);
        return result;
    }

    token = token == null ? mToken : token;

    //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
    apiPath = String.Format("{0}/{1}", mApiHost, apiPath);

    //2、根据该API接口 的请求参数中 得知需要创建两个URI Parameters String类型参数，所以该参数直接跟在apiPath中
    apiPath = apiPath.Replace("{gatewaytag}", gatewayTag);//将API地址中的{gatewayTag}替换成真实设备标识
    apiPath = apiPath.Replace("{apitag}", apiTag);//将API地址中的{apiTag}替换成真实传感器API标识

    //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
    HttpReqEntity req = new HttpReqEntity();
    req.Method = HttpMethod.GET;
    req.Headers.Add("AccessToken", token);

    //4、定义该API接口返回的对象
    result = RequestAPIHelper.RequestServer<HttpReqEntity, GatewayDeviceDataDTO>(apiPath, req);

    return result;
}

/// <summary>
/// 获取某个传感器的最新值
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="apiTag">传感器ApiTag</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
public ResultMsg<GatewayDeviceDataDTO> GetSensorNewestData(String gatewayTag, String apiTag, String token = null)
{
    return GetDeviceNewestData(NLECloudAPIUrl.SensorNewestDatasUrl, gatewayTag, apiTag, token);
}

/// <summary>
/// 获取某个执行器的最新值
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="apiTag">执行器ApiTag</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
public ResultMsg<GatewayDeviceDataDTO> GetActuatorNewestData(String gatewayTag, String apiTag, String token = null)
{
    return GetDeviceNewestData(NLECloudAPIUrl.ActuatorNewestDatasUrl, gatewayTag, apiTag, token);
}

/// <summary>
/// 获取某个设备的历史数据
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private ResultMsg<IEnumerable<GatewayDeviceChartDataDTO>> GetDeviceHistoryData(String apiPath, String gatewayTag, String apiTag, GatewayDeviceHistoryQryParas query, String token = null)
{
    var result = new ResultMsg<IEnumerable<GatewayDeviceChartDataDTO>>();

    //验证
    var vry = ApiTagVerify(gatewayTag, apiTag, ref token);
    if (!vry.IsSuccess())
    {
        vry.CopyTo(result);
        return result;
    }

    //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
    apiPath = String.Format("{0}/{1}", mApiHost, apiPath);

    //2、根据该API接口 的请求参数中 得知需要创建两个URI Parameters String类型参数，所以该参数直接跟在apiPath中
    apiPath = apiPath.Replace("{gatewaytag}", gatewayTag.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
    apiPath = apiPath.Replace("{apitag}", apiTag.Trim());//将API地址中的{apiTag}替换成真实传感器API标识
    //2、同时再自行定义这两个URI Parameters String类型参数：Method、TimeAgo
    apiPath += String.Format("?{0}&{1}"
        , "Method=" + query.Method        //查询方式（1：XX分钟内 2：XX小时内 3：XX天内 4：XX周内 5：XX月内 6：按startDate与endDate指定日期查询）
        , "TimeAgo=" + query.TimeAgo      //与Method配对使用表示"多少TimeAgo Method内"的数据，例：(Method=2,TimeAgo=30)表示30小时内的历史数据
        );

    //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
    HttpReqEntity req = new HttpReqEntity();
    req.Method = HttpMethod.GET;
    req.Headers.Add("AccessToken", token);

    //4、定义该API接口返回的对象
    result = RequestAPIHelper.RequestServer<HttpReqEntity, IEnumerable<GatewayDeviceChartDataDTO>>(apiPath, req);

    return result;
}

/// <summary>
/// 获取某个传感器的历史数据
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="apiTag">传感器ApiTag</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
public ResultMsg<IEnumerable<GatewayDeviceChartDataDTO>> GetSensorHistoryData(String gatewayTag, String apiTag, GatewayDeviceHistoryQryParas query, String token = null)
{
    return GetDeviceHistoryData(NLECloudAPIUrl.SensorHistoryDataUrl, gatewayTag, apiTag, query, token);
}

/// <summary>
/// 获取某个执行器的历史数据
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="apiTag">执行器ApiTag</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
public ResultMsg<IEnumerable<GatewayDeviceChartDataDTO>> GetActuatorHistoryData(String gatewayTag, String apiTag, GatewayDeviceHistoryQryParas query, String token = null)
{
    return GetDeviceHistoryData(NLECloudAPIUrl.ActuatorHistoryDataUrl, gatewayTag, apiTag, query, token);
}


/// <summary>
/// 获取某个设备的历史分页数据
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private ResultMsg<ListPagerSet<GatewayDeviceListDataDTO>> GetDeviceHistoryPagerData(String apiPath, String gatewayTag, String apiTag , GatewayDeviceHistoryPagerQryParas query , String token = null)
{
    var result = new ResultMsg<ListPagerSet<GatewayDeviceListDataDTO>>();

    //验证
    var vry = ApiTagVerify(gatewayTag, apiTag, ref token);
    if (!vry.IsSuccess())
    {
        vry.CopyTo(result);
        return result;
    }


    //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
    apiPath = String.Format("{0}/{1}", mApiHost, apiPath);

    //2、根据该API接口 的请求参数中 得知需要创建一个URI Parameters String类型参数，所以该参数直接跟在apiPath中
    apiPath = apiPath.Replace("{gatewaytag}", gatewayTag.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
    apiPath = apiPath.Replace("{apitag}", apiTag.Trim());//将API地址中的{apiTag}替换成真实传感器API标识
    //2、同时再自行定义这四个URI Parameters String类型参数：StartDate、EndDate、PageIndex、PageSize
    apiPath += String.Format("?{0}&{1}&{2}&{3}"
        , "PageIndex=" + query.PageIndex                             //这里自定义当前要查询的页码数
        , "PageSize=" + query.PageSize                           //这里定义每页要查询的数量
        , "StartDate=" + query.StartDate                    //这里定义从某个时间段开始查询，现在定义为30天前
        , "EndDate=" + query.EndDate);                      //这里定义查询到某个时间点结束，现在定义为当前时间

    //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
    HttpReqEntity req = new HttpReqEntity();
    req.Method = HttpMethod.GET;
    req.Headers.Add("AccessToken", token);

    //4、定义该API接口返回的对象
    result = RequestAPIHelper.RequestServer<HttpReqEntity, ListPagerSet<GatewayDeviceListDataDTO>>(apiPath, req);

    return result;
}

/// <summary>
/// 获取某个传感器的历史分页数据
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="apiTag">传感器ApiTag</param>
/// <param name="query">分页查询参数</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
/// <returns></returns>
public ResultMsg<ListPagerSet<GatewayDeviceListDataDTO>> GetSensorHistoryPagerData(String gatewayTag, String apiTag, GatewayDeviceHistoryPagerQryParas query, String token = null)
{
    return GetDeviceHistoryPagerData(NLECloudAPIUrl.SensorHistoryPagerDataUrl, gatewayTag, apiTag,query, token);
}

/// <summary>
/// 获取某个执行器的历史分页数据
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="apiTag">执行器ApiTag</param>
/// <param name="query">分页查询参数</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
/// <returns></returns>
public ResultMsg<ListPagerSet<GatewayDeviceListDataDTO>> GetActuatorHistoryPagerData(String gatewayTag, String apiTag, GatewayDeviceHistoryPagerQryParas query, String token = null)
{
    return GetDeviceHistoryPagerData(NLECloudAPIUrl.ActuatorHistoryPagerDataUrl, gatewayTag, apiTag,query, token);
}


/// <summary>
/// 获取某个项目的信息
/// </summary>
/// <param name="projectTag">项目标识</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
/// <returns></returns>
public ResultMsg<ProjectInfoDTO> GetProjectInfo(String projectTag , String token)
{
    var result = new ResultMsg<ProjectInfoDTO>();

    token = token == null ? this.mToken : token;

    if (token.Trim() == "")
    {
        return result.SetFailure("请输入API请求的头部参数AccessToken值");
    }
    if (projectTag.Trim() == "")
    {
        return result.SetFailure("请输入某个目的项目标识！");
    }

    //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
    String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.ProjectInfoUrl);

    //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
    apiPath = apiPath.Replace("{tag}", projectTag.Trim());//将API地址中的{gatewayTag}替换成真实设备标识

    //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
    HttpReqEntity req = new HttpReqEntity();
    req.Method = HttpMethod.GET;
    req.Headers.Add("AccessToken", token);

    //4、定义该API接口返回的对象
    result = RequestAPIHelper.RequestServer<HttpReqEntity, ProjectInfoDTO>(apiPath, req);

    return result;
}


/// <summary>
/// 控制某个执行器
/// </summary>
/// <param name="gatewayTag">设备标识</param>
/// <param name="apiTag">执行器ApiTag</param>
/// <param name="data">控制值</param>
/// <param name="token">请求TOKEN(为空时请先调用用户登录接口缓存Token上下文)</param>
/// <returns></returns>
public ResultMsg<Result> Control(String gatewayTag, String apiTag, String data, String token = null)
{
    var result = new ResultMsg<Result>();

    //验证
    var vry = ApiTagVerify(gatewayTag, apiTag, ref token);
    if (!vry.IsSuccess())
    {
        vry.CopyTo(result);
        return result;
    }

    if (data.Trim() == "")
    {
        return result.SetFailure("控制执行器请输入控制值！");
    }

    //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
    String apiPath = String.Format("{0}/{1}", mApiHost, NLECloudAPIUrl.ControlUrl);

    //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
    apiPath = apiPath.Replace("{gatewaytag}", gatewayTag.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
    apiPath = apiPath.Replace("{apitag}", apiTag.Trim());//将API地址中的{apiTag}替换成真实设备apiTag
    apiPath = apiPath.Replace("{data}", data);//将API地址中的{data}替换成要控制的值

    //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
    HttpReqEntity req = new HttpReqEntity();
    req.Method = HttpMethod.POST;
    req.Headers.Add("AccessToken", token);

    //4、定义该API接口返回的对象
    result = RequestAPIHelper.RequestServer<HttpReqEntity>(apiPath, req);

    return result;
}
*/
        #endregion
    }
}
