using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NLECloudSDKSample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string OUT_STRING = "【{0}】{1}\r\n";

        #region -- 构造函数 -- 
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            AppendAPIUrl(new TextBox[] { this.txtLoginAPI, 
                this.txtGatewayInfoAPI, this.txtSensorListAPI ,this.txtSensorAPI,this.txtActuatorListAPI,this.txtActuatorAPI,
                this.txtCameraListAPI,this.txtCameraAPI,
                this.txtOnOfflineAPI,this.txtHistoryPagerOnOfflineAPI,this.txtStatusAPI,this.txtNewestDatasAPI,
                this.txtNewestDataAPI,this.txtHistoryDataAPI,this.txtHistoryPagerDataAPI,
                this.txtActuatorNewestDataAPI,this.txtActuatorHistoryDataAPI,this.txtActuatorHistoryPagerDataAPI,
                this.txtProjectInfoAPI,this.txtControlAPI
            });
        }

        #endregion


        #region -- 私有方法 -- 

        /// <summary>
        /// AppendAPIUrl
        /// </summary>
        /// <param name="tb"></param>
        private void AppendAPIUrl(TextBox[] tbList)
        {
            foreach (TextBox tb in tbList)
            {
                tb.Text = RequestAPIHelper.ServerDomain + "/" + tb.Text;
            }
        }

        /// <summary>
        /// 输入消息
        /// </summary>
        /// <param name="str"></param>
        private void AppendRTxt(String str , bool Response = false)
        {
            RichTextBox tmp = mTextBoxReq;
            if (Response) tmp = mTextBox;
            tmp.Dispatcher.Invoke(() =>
            {
                tmp.AppendText(string.Format(OUT_STRING, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), str));
            });
        }

        /// <summary>
        /// 统一输出
        /// </summary>
        /// <param name="qry"></param>
        /// <param name="apiPath"></param>
        /// <param name="reqJSON"></param>
        /// <param name="resJSON"></param>
        private void Out(Result qry , String apiPath , String reqJSON = null )
        {
            //请求成功
            if (qry.IsSuccess())
            {
                AppendRTxt("请求" + apiPath + "成功，请求JSON:" + (reqJSON == null ? "" : reqJSON));
                AppendRTxt("请求" + apiPath + "返回JSON：" + qry.Msg, true);
            }
            else
            {
                AppendRTxt("请求" + apiPath + "失败：" + qry.Msg + "，请求JSON：" + reqJSON);
            }
        }

        /// <summary>
        /// 设备TAB相关接口请求统一验证
        /// </summary>
        /// <returns></returns>
        private Result DeviceApiReqverify()
        {
            Result result = new Result();

            if (this.txtToken.Text.Trim() == "")
            {
                result.SetFailure("请先进行用户登录以便获取Token！");
                MessageBox.Show(result.Msg);
            }
            if (txtGatewayTag.Text.Trim() == "")
            {
                result.SetFailure("请在‘获取某个网关信息’一栏输入你在云平台上已添加的某个设备标识！");
                MessageBox.Show(result.Msg);
            }

            return result;
        }

        /// <summary>
        /// 网关数据TAB相关接口请求统一验证
        /// </summary>
        /// <returns>成功返回GatewayTag</returns>
        private Result GatewayDataApiReqverify()
        {
            Result result = new Result();

            if (this.txtToken.Text.Trim() == "")
            {
                result.SetFailure("请先进行用户登录以便获取Token！");
                MessageBox.Show(result.Msg);
            }
            if (txtGatewayTag.Text.Trim() == "" && txtTab1GatewayTag.Text.Trim() == "")
            {
                result.SetFailure("请在‘设备标识’框输入你在云平台上已添加的某个设备标识！");
                MessageBox.Show(result.Msg);
            }
            else
            {
                if (txtGatewayTag.Text.Trim() != "" && txtTab1GatewayTag.Text.Trim() == "")
                {
                    txtTab1GatewayTag.Text = txtGatewayTag.Text.Trim();
                }
                result.Msg = txtTab1GatewayTag.Text;
            }

            return result;
        }

        /// <summary>
        /// 网关数据TAB相关接口请求统一验证
        /// </summary>
        /// <returns></returns>
        private Result DeviceDataApiReqverify()
        {
            Result result = new Result();

            if (this.txtToken.Text.Trim() == "")
            {
                result.SetFailure("请先进行用户登录以便获取Token！");
                MessageBox.Show(result.Msg);
            }
            if (txtGatewayTag.Text.Trim() == "" && txtTab2GatewayTag.Text.Trim() == "")
            {
                result.SetFailure("请在‘设备标识’框输入你在云平台上已添加的某个设备标识！");
                MessageBox.Show(result.Msg);
            }
            else
            {
                if (txtGatewayTag.Text.Trim() != "" && txtTab2GatewayTag.Text.Trim() == "")
                {
                    txtTab2GatewayTag.Text = txtGatewayTag.Text.Trim();
                }
                result.Msg = txtTab1GatewayTag.Text;
            }

            return result;
        }

        #endregion

        #region -- 云平台登录 -- 
        
        /// <summary>
        /// 云平台登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtUserName.Text.Trim() == "" || this.txtPasswd.Password.Trim() == "")
            {
                MessageBox.Show("请输入登录用户名和密码！");
                return;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的 帐号API接口 中的 用户登录得知
            String apiPath = txtLoginAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建个Body Parameters的参数对象，下面创建一个该类的对象
            AccountLoginDTO DTO = new AccountLoginDTO();
            DTO.Account = txtUserName.Text.Trim(); //帐号为云平台注册的手机号或用户名等
            DTO.Password = txtPasswd.Password.Trim();//密码为云平台注册的帐号密码

            //3、定义该API接口返回的对象,初始化为空
            ResultMsg<AccountLoginResultDTO> qry = null;
            qry = RequestAPIHelper.RequestServer<AccountLoginDTO, AccountLoginResultDTO>(apiPath, DTO);

            Out(qry, apiPath, DTO.DTOToJson());

            //请求成功
            if (qry.IsSuccess())
            {
                txtToken.Text = qry.ResultObj.AccessToken;
            }
        }

        #endregion



        #region -- 获取某个网关的信息 --
        
        /// <summary>
        /// 获取某个网关的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGatewayInfo_Click(object sender, RoutedEventArgs e)
        {
            if (!DeviceApiReqverify().IsSuccess())
                return;

             //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtGatewayInfoAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            String gatewayTag = txtGatewayTag.Text;
            apiPath = apiPath.Replace("{gatewayTag}", gatewayTag);//将API地址中的{gatewayTag}替换成真实设备标识

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);

            //4、定义该API接口返回的对象
            ResultMsg<GatewayInfoDTO> qry = RequestAPIHelper.RequestServer<HttpReqEntity, GatewayInfoDTO>(apiPath, req);

            Out(qry, apiPath);

        }

        #endregion

        #region -- 获取某个网关的传感器列表 --

        /// <summary>
        /// 获取某个网关的传感器列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSensorList_Click(object sender, RoutedEventArgs e)
        {
            if (!DeviceApiReqverify().IsSuccess())
                return;

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtSensorListAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            String gatewayTag = txtGatewayTag.Text;
            apiPath = apiPath.Replace("{gatewayTag}", gatewayTag);//将API地址中的{gatewayTag}替换成真实设备标识

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);

            //4、定义该API接口返回的对象,由于是列表所以要用IEnumerable
            ResultMsg<IEnumerable<GatewayDeviceSensorInfoDTO>> qry = RequestAPIHelper.RequestServer<HttpReqEntity, IEnumerable<GatewayDeviceSensorInfoDTO>>(apiPath, req);

            Out(qry, apiPath);

        }

        #endregion

        #region -- 获取某个传感器的信息 -- 
        
        /// <summary>
        /// 获取某个传感器的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSensor_Click(object sender, RoutedEventArgs e)
        {
            if (!DeviceApiReqverify().IsSuccess())
                return;

            if (this.txtSensorApiTag.Text.Trim() == "")
            {
                MessageBox.Show("请输入你在云平台上已添加的某个传感器API标识！");
                return;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtSensorAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建两个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{gatewayTag}", txtGatewayTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath = apiPath.Replace("{apiTag}", txtSensorApiTag.Text.Trim());//将API地址中的{apiTag}替换成真实传感器API标识

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);

            //4、定义该API接口返回的对象
            ResultMsg<GatewayDeviceSensorInfoDTO> qry = RequestAPIHelper.RequestServer<HttpReqEntity, GatewayDeviceSensorInfoDTO>(apiPath, req);

            Out(qry, apiPath);
        }
        #endregion

        #region -- 获取某个网关的执行器列表 --

        /// <summary>
        /// 获取某个网关的执行器列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnActuatorList_Click(object sender, RoutedEventArgs e)
        {
            if (!DeviceApiReqverify().IsSuccess())
                return;

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtActuatorListAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            String gatewayTag = txtGatewayTag.Text;
            apiPath = apiPath.Replace("{gatewayTag}", gatewayTag);//将API地址中的{gatewayTag}替换成真实设备标识

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);

            //4、定义该API接口返回的对象,由于是列表所以要用IEnumerable
            ResultMsg<IEnumerable<GatewayDeviceActuatorInfoDTO>> qry = RequestAPIHelper.RequestServer<HttpReqEntity, IEnumerable<GatewayDeviceActuatorInfoDTO>>(apiPath, req);

            Out(qry, apiPath);

        }

        #endregion

        #region -- 获取某个执行器的信息 --

        /// <summary>
        /// 获取某个执行器的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnActuator_Click(object sender, RoutedEventArgs e)
        {
            if (!DeviceApiReqverify().IsSuccess())
                return;

            if (this.txtActuatorApiTag.Text.Trim() == "")
            {
                MessageBox.Show("请输入你在云平台上已添加的某个执行器API标识！");
                return;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtActuatorAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建两个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{gatewayTag}", txtGatewayTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath = apiPath.Replace("{apiTag}", txtActuatorApiTag.Text.Trim());//将API地址中的{apiTag}替换成真实执行器API标识

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);

            //4、定义该API接口返回的对象
            ResultMsg<GatewayDeviceActuatorInfoDTO> qry = RequestAPIHelper.RequestServer<HttpReqEntity, GatewayDeviceActuatorInfoDTO>(apiPath, req);

            Out(qry, apiPath);
        }
        #endregion

        #region -- 获取某个网关的摄像头列表 --

        /// <summary>
        /// 获取某个网关的摄像头列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCameraList_Click(object sender, RoutedEventArgs e)
        {
            if (!DeviceApiReqverify().IsSuccess())
                return;

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtCameraListAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            String gatewayTag = txtGatewayTag.Text;
            apiPath = apiPath.Replace("{gatewayTag}", gatewayTag);//将API地址中的{gatewayTag}替换成真实设备标识

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);

            //4、定义该API接口返回的对象,由于是列表所以要用IEnumerable
            ResultMsg<IEnumerable<GatewayDeviceCameraInfoDTO>> qry = RequestAPIHelper.RequestServer<HttpReqEntity, IEnumerable<GatewayDeviceCameraInfoDTO>>(apiPath, req);

            Out(qry, apiPath);

        }

        #endregion

        #region -- 获取某个摄像头的信息 --

        /// <summary>
        /// 获取某个摄像头的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCamera_Click(object sender, RoutedEventArgs e)
        {
            if (!DeviceApiReqverify().IsSuccess())
                return;

            if (this.txtCameraApiTag.Text.Trim() == "")
            {
                MessageBox.Show("请输入你在云平台上已添加的某个摄像头API标识！");
                return;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtCameraAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建两个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{gatewayTag}", txtGatewayTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath = apiPath.Replace("{apiTag}", txtCameraApiTag.Text.Trim());//将API地址中的{apiTag}替换成真实摄像头API标识

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);

            //4、定义该API接口返回的对象
            ResultMsg<GatewayDeviceCameraInfoDTO> qry = RequestAPIHelper.RequestServer<HttpReqEntity, GatewayDeviceCameraInfoDTO>(apiPath, req);

            Out(qry, apiPath);
        }

        #endregion




        #region -- 获取某个网关的当前在/离线状态 -- 
        
        /// <summary>
        /// 获取某个网关的当前在/离线状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOnOffline_Click(object sender, RoutedEventArgs e)
        {
            var result = GatewayDataApiReqverify();
            if (!result.IsSuccess())
                return;

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtOnOfflineAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建两个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{gatewayTag}", result.Msg);//将API地址中的{gatewayTag}替换成真实设备标识

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);

            //4、定义该API接口返回的对象
            ResultMsg<GatewayOnlineDataDTO> qry = RequestAPIHelper.RequestServer<HttpReqEntity, GatewayOnlineDataDTO>(apiPath, req);

            Out(qry, apiPath);
        }

        #endregion

        #region -- 获取某个网关的历史分页在/离线状态 --

        /// <summary>
        /// 获取某个网关的历史分页在/离线状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHistoryPagerOnOffline_Click(object sender, RoutedEventArgs e)
        {
            var result = GatewayDataApiReqverify();
            if (!result.IsSuccess())
                return;

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtHistoryPagerOnOfflineAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建一个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{gatewayTag}", result.Msg);//将API地址中的{gatewayTag}替换成真实设备标识
            //2、同时得知也需要一个GatewayOnOfflineHistoryQryParas 的Body Parameters请求参数
            //其中包含四个查询子属性StartDate、EndDate、PageIndex、PageSize
            //根据自己自行定义
            GatewayOnOfflineHistoryQryParas reqBody = new GatewayOnOfflineHistoryQryParas()
            {
                PageIndex = 1,//这里自定义当前要查询的页码数
                PageSize = 20,//这里定义每页要查询的数量
                StartDate = DateTime.Now.AddDays(-30),//这里定义从某个时间段开始查询，现在定义为30天前
                EndDate = DateTime.Now,//这里定义查询到某个时间点结束，现在定义为当前时间
            };

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);

            //这里跟之前设备TAB相关的接口不一样区别是，因为该接口要传递Body Parameters，所以把reqBody当包体数据赋给req
            req.Datas = JsonFormatter.Serialize(reqBody);

            //4、定义该API接口返回的对象
            ResultMsg<ListPagerSet<GatewayOnlineRecordListDTO>> qry = RequestAPIHelper.RequestServer<HttpReqEntity, ListPagerSet<GatewayOnlineRecordListDTO>>(apiPath, req);

            Out(qry, apiPath);
        }

        #endregion

        #region -- 获取某个网关的当前启/禁状态 --

        /// <summary>
        /// 获取某个网关的当前启/禁状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStatus_Click(object sender, RoutedEventArgs e)
        {
            var result = GatewayDataApiReqverify();
            if (!result.IsSuccess())
                return;

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtStatusAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建两个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{gatewayTag}", result.Msg);//将API地址中的{gatewayTag}替换成真实设备标识

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);

            //4、定义该API接口返回的对象
            ResultMsg<Boolean> qry = RequestAPIHelper.RequestServer<HttpReqEntity, Boolean>(apiPath, req);

            Out(qry, apiPath);
        }

        #endregion

        #region -- 获取某个网关的所有传感器、执行器最新值 --

        /// <summary>
        /// 获取某个网关的所有传感器、执行器最新值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_NewestDatas_Click(object sender, RoutedEventArgs e)
        {
            var result = GatewayDataApiReqverify();
            if (!result.IsSuccess())
                return;

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtNewestDatasAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建两个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{gatewayTag}", result.Msg);//将API地址中的{gatewayTag}替换成真实设备标识

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);

            //4、定义该API接口返回的对象
            ResultMsg<IEnumerable<GatewayDeviceDataDTO>> qry = RequestAPIHelper.RequestServer<HttpReqEntity, IEnumerable<GatewayDeviceDataDTO>>(apiPath, req);

            Out(qry, apiPath);
        }

        #endregion



        #region -- 获取某个传感器的最新值 --

        /// <summary>
        /// 获取某个传感器的最新值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewestData_Click(object sender, RoutedEventArgs e)
        {
            if (!DeviceDataApiReqverify().IsSuccess())
                return;

            if (this.txtNewestDataApiTag.Text.Trim() == "")
            {
                MessageBox.Show("请输入你在云平台上已添加的某个传感器API标识！");
                return;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtNewestDataAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建两个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{gatewayTag}", txtTab2GatewayTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath = apiPath.Replace("{apiTag}", txtNewestDataApiTag.Text.Trim());//将API地址中的{apiTag}替换成真实传感器API标识

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);

            //4、定义该API接口返回的对象
            ResultMsg<GatewayDeviceDataDTO> qry = RequestAPIHelper.RequestServer<HttpReqEntity, GatewayDeviceDataDTO>(apiPath, req);

            Out(qry, apiPath);
        }

        #endregion

        #region -- 获取某个传感器的历史数据 -- 
        
        /// <summary>
        /// 获取某个传感器的历史数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHistoryData_Click(object sender, RoutedEventArgs e)
        {
            if (!DeviceDataApiReqverify().IsSuccess())
                return;

            if (this.txtHistoryDataApiTag.Text.Trim() == "")
            {
                MessageBox.Show("请输入你在云平台上已添加的某个传感器API标识！");
                return;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtHistoryDataAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建两个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{gatewayTag}", txtTab2GatewayTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath = apiPath.Replace("{apiTag}", txtHistoryDataApiTag.Text.Trim());//将API地址中的{apiTag}替换成真实传感器API标识
            //2、同时得知也需要一个GatewayDeviceHistoryQryParas 的Body Parameters请求参数
            //根据自己自行定义
            GatewayDeviceHistoryQryParas reqBody = new GatewayDeviceHistoryQryParas()
            {
                Method = 2,//查询方式（1：XX分钟内 2：XX小时内 3：XX天内 4：XX周内 5：XX月内 6：按startDate与endDate指定日期查询）
                TimeAgo = 24,//与Method配对使用表示"多少TimeAgo Method内"的数据，例：(Method=2,TimeAgo=30)表示30小时内的历史数据
            };

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);
            //这里跟之前设备TAB相关的接口不一样区别是，因为该接口要传递Body Parameters，所以把reqBody当包体数据赋给req
            req.Datas = JsonFormatter.Serialize(reqBody);

            //4、定义该API接口返回的对象
            ResultMsg<IEnumerable<GatewayDeviceChartDataDTO>> qry = RequestAPIHelper.RequestServer<HttpReqEntity, IEnumerable<GatewayDeviceChartDataDTO>>(apiPath, req);

            Out(qry, apiPath);
        }

        #endregion

        #region -- 获取某个传感器的历史分页数据 --

        /// <summary>
        /// 获取某个传感器的历史分页数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHistoryPagerData_Click(object sender, RoutedEventArgs e)
        {
            if (!DeviceDataApiReqverify().IsSuccess())
                return;

            if (this.txtHistoryPagerDataApiTag.Text.Trim() == "")
            {
                MessageBox.Show("请输入你在云平台上已添加的某个传感器API标识！");
                return;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtHistoryPagerDataAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建一个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{gatewayTag}", txtTab2GatewayTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath = apiPath.Replace("{apiTag}", txtHistoryPagerDataApiTag.Text.Trim());//将API地址中的{apiTag}替换成真实传感器API标识
            //2、同时得知也需要一个GatewayDeviceHistoryPagerQryParas 的Body Parameters请求参数
            //其中包含四个查询子属性StartDate、EndDate、PageIndex、PageSize
            //根据自己自行定义
            GatewayDeviceHistoryPagerQryParas reqBody = new GatewayDeviceHistoryPagerQryParas()
            {
                PageIndex = 1,//这里自定义当前要查询的页码数
                PageSize = 20,//这里定义每页要查询的数量
                StartDate = DateTime.Now.AddDays(-30),//这里定义从某个时间段开始查询，现在定义为30天前
                EndDate = DateTime.Now,//这里定义查询到某个时间点结束，现在定义为当前时间
            };

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);

            //这里跟之前设备TAB相关的接口不一样区别是，因为该接口要传递Body Parameters，所以把reqBody当包体数据赋给req
            req.Datas = JsonFormatter.Serialize(reqBody);

            //4、定义该API接口返回的对象
            ResultMsg<ListPagerSet<GatewayDeviceListDataDTO>> qry = RequestAPIHelper.RequestServer<HttpReqEntity, ListPagerSet<GatewayDeviceListDataDTO>>(apiPath, req);

            Out(qry, apiPath);
        }

        #endregion

        #region -- 获取某个执行器的最新值 --

        /// <summary>
        /// 获取某个执行器的最新值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnActuatorNewestData_Click(object sender, RoutedEventArgs e)
        {
            if (!DeviceDataApiReqverify().IsSuccess())
                return;

            if (this.txtActuatorNewestDataApiTag.Text.Trim() == "")
            {
                MessageBox.Show("请输入你在云平台上已添加的某个执行器API标识！");
                return;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtActuatorNewestDataAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建两个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{gatewayTag}", txtTab2GatewayTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath = apiPath.Replace("{apiTag}", txtActuatorNewestDataApiTag.Text.Trim());//将API地址中的{apiTag}替换成真实传感器API标识

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);

            //4、定义该API接口返回的对象
            ResultMsg<GatewayDeviceDataDTO> qry = RequestAPIHelper.RequestServer<HttpReqEntity, GatewayDeviceDataDTO>(apiPath, req);

            Out(qry, apiPath);
        }

        #endregion

        #region -- 获取某个执行器的历史数据 --

        /// <summary>
        /// 获取某个执行器的历史数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnActuatorHistoryData_Click(object sender, RoutedEventArgs e)
        {
            if (!DeviceDataApiReqverify().IsSuccess())
                return;

            if (this.txtActuatorHistoryDataApiTag.Text.Trim() == "")
            {
                MessageBox.Show("请输入你在云平台上已添加的某个执行器API标识！");
                return;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtActuatorHistoryDataAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建两个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{gatewayTag}", txtTab2GatewayTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath = apiPath.Replace("{apiTag}", txtActuatorHistoryDataApiTag.Text.Trim());//将API地址中的{apiTag}替换成真实传感器API标识

            //2、同时得知也需要一个GatewayDeviceHistoryQryParas 的Body Parameters请求参数
            //根据自己自行定义
            GatewayDeviceHistoryQryParas reqBody = new GatewayDeviceHistoryQryParas()
            {
                Method = 2,//查询方式（1：XX分钟内 2：XX小时内 3：XX天内 4：XX周内 5：XX月内 6：按startDate与endDate指定日期查询）
                TimeAgo = 24,//与Method配对使用表示"多少TimeAgo Method内"的数据，例：(Method=2,TimeAgo=30)表示30小时内的历史数据
            };

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);
            //这里跟之前设备TAB相关的接口不一样区别是，因为该接口要传递Body Parameters，所以把reqBody当包体数据赋给req
            req.Datas = JsonFormatter.Serialize(reqBody);

            //4、定义该API接口返回的对象
            ResultMsg<IEnumerable<GatewayDeviceChartDataDTO>> qry = RequestAPIHelper.RequestServer<HttpReqEntity, IEnumerable<GatewayDeviceChartDataDTO>>(apiPath, req);

            Out(qry, apiPath);
        }

        #endregion

        #region -- 获取某个执行器的历史分页数据 --

        /// <summary>
        /// 获取某个执行器的历史分页数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnActuatorHistoryPagerData_Click(object sender, RoutedEventArgs e)
        {
            if (!DeviceDataApiReqverify().IsSuccess())
                return;

            if (this.txtActuatorHistoryPagerDataApiTag.Text.Trim() == "")
            {
                MessageBox.Show("请输入你在云平台上已添加的某个执行器API标识！");
                return;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtActuatorHistoryPagerDataAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建一个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{gatewayTag}", txtTab2GatewayTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath = apiPath.Replace("{apiTag}", txtActuatorHistoryPagerDataApiTag.Text.Trim());//将API地址中的{apiTag}替换成真实传感器API标识
            //2、同时得知也需要一个GatewayDeviceHistoryPagerQryParas 的Body Parameters请求参数
            //其中包含四个查询子属性StartDate、EndDate、PageIndex、PageSize
            //根据自己自行定义
            GatewayDeviceHistoryPagerQryParas reqBody = new GatewayDeviceHistoryPagerQryParas()
            {
                PageIndex = 1,//这里自定义当前要查询的页码数
                PageSize = 20,//这里定义每页要查询的数量
                StartDate = DateTime.Now.AddDays(-30),//这里定义从某个时间段开始查询，现在定义为30天前
                EndDate = DateTime.Now,//这里定义查询到某个时间点结束，现在定义为当前时间
            };

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);

            //这里跟之前设备TAB相关的接口不一样区别是，因为该接口要传递Body Parameters，所以把reqBody当包体数据赋给req
            req.Datas = JsonFormatter.Serialize(reqBody);

            //4、定义该API接口返回的对象
            ResultMsg<ListPagerSet<GatewayDeviceListDataDTO>> qry = RequestAPIHelper.RequestServer<HttpReqEntity, ListPagerSet<GatewayDeviceListDataDTO>>(apiPath, req);

            Out(qry, apiPath);
        }

        #endregion



        #region -- 获取某个项目的信息 -- 
        
        /// <summary>
        /// 获取某个项目的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProjectInfo_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtToken.Text.Trim() == "")
            {
                MessageBox.Show("请先进行用户登录以便获取Token！");
                return;
            }
            if (txtProjectTag.Text.Trim() == "")
            {
                MessageBox.Show("请在‘项目标识’框输入你在云平台上已添加的某个项目标识！");
                return;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtProjectInfoAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{tag}", txtProjectTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);

            //4、定义该API接口返回的对象
            ResultMsg<ProjectInfoDTO> qry = RequestAPIHelper.RequestServer<HttpReqEntity, ProjectInfoDTO>(apiPath, req);

            Out(qry, apiPath);
        }

        #endregion

        #region -- 控制某个执行器 -- 
        
        /// <summary>
        /// 控制某个执行器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnControl_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtToken.Text.Trim() == "")
            {
                MessageBox.Show("请先进行用户登录以便获取Token！");
                return;
            }
            if (txtTab5GatewayTag.Text.Trim() == "")
            {
                MessageBox.Show("请在‘设备标识’框输入你在云平台上已添加的某个设备标识！");
                return;
            }
            if (txtControlApiTag.Text.Trim() == "")
            {
                MessageBox.Show("请在‘ApiTag’框输入你在云平台上已添加的某个执行器ApiTag！");
                return;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtControlAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{gatewayTag}", txtTab5GatewayTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath = apiPath.Replace("{apiTag}", txtControlApiTag.Text.Trim());//将API地址中的{apiTag}替换成真实设备apiTag
            apiPath = apiPath.Replace("{data}", txtControlData.Text.Trim());//将API地址中的{data}替换成要控制的值

            //3、由于调用该API需要Token，所以我们定义了一个通用的对象HttpReqEntity，在AccessToken当成头部请求信息提交过去
            HttpReqEntity req = new HttpReqEntity();
            req.Method = HttpMethod.POST;
            req.Headers.Add("AccessToken", txtToken.Text);

            //4、定义该API接口返回的对象
            ResultMsg<Result> qry = RequestAPIHelper.RequestServer<HttpReqEntity>(apiPath, req);

            Out(qry, apiPath);
        }

        #endregion
    }
}
