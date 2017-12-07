using NLECloudSDK;
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
        //默认为新大陆物联网云平台API域名，测试环境或私有云请更改为自己的
        public static String API_HOST = ApplicationSettings.Get("ApiHost"); 
        public const string OUT_STRING = "【{0}】{1}\r\n";
        NLECloudAPI SDK = null;

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

            SDK = new NLECloudAPI(API_HOST);
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
                tb.Text = API_HOST + "/" + tb.Text;
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

            //1、根据该API接口 的请求参数中 得知需要创建个Body Parameters的参数对象，下面创建一个该类的对象
            AccountLoginDTO DTO = new AccountLoginDTO();
            DTO.Account = txtUserName.Text.Trim(); //帐号为云平台注册的手机号或用户名等
            DTO.Password = txtPasswd.Password.Trim();//密码为云平台注册的帐号密码

            //3、定义该API接口返回的对象,初始化为空
            ResultMsg<AccountLoginResultDTO> qry = SDK.UserLogin(DTO);

            Out(qry, txtLoginAPI.Text, DTO.DTOToJson());

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

            String gatewayTag = txtGatewayTag.Text;
            String apiPath = this.txtGatewayInfoAPI.Text.Replace("{gatewayTag}", gatewayTag);

            var qry = SDK.GetGatewayInfo(gatewayTag, txtToken.Text);

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

            String gatewayTag = txtGatewayTag.Text;
            String apiPath = this.txtSensorListAPI.Text.Replace("{gatewayTag}", gatewayTag);

            var qry = SDK.GetSensorList(gatewayTag, txtToken.Text);

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


            var qry = SDK.GetSensorInfo(txtGatewayTag.Text.Trim(), txtSensorApiTag.Text.Trim(), txtToken.Text);

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

            String gatewayTag = txtGatewayTag.Text;
            String apiPath = this.txtActuatorListAPI.Text.Replace("{gatewayTag}", gatewayTag);

            var qry = SDK.GetActuatorList(gatewayTag, txtToken.Text);

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
            apiPath = apiPath.Replace("{apiTag}", txtActuatorApiTag.Text.Trim());//将API地址中的{apiTag}替换成真实传感器API标识


            var qry = SDK.GetActuatorInfo(txtGatewayTag.Text.Trim(), txtActuatorApiTag.Text.Trim(), txtToken.Text);

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

            String gatewayTag = txtGatewayTag.Text;
            String apiPath = this.txtCameraListAPI.Text.Replace("{gatewayTag}", gatewayTag);

            var qry = SDK.GetCameraList(gatewayTag, txtToken.Text);

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
                MessageBox.Show("请输入你在云平台上已添加的某个传感器API标识！");
                return;
            }

            //1、先定义该API接口路径，可以从http://api.nlecloud.com/页面的得知
            String apiPath = this.txtCameraAPI.Text;

            //2、根据该API接口 的请求参数中 得知需要创建两个URI Parameters String类型参数，所以该参数直接跟在apiPath中
            apiPath = apiPath.Replace("{gatewayTag}", txtGatewayTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath = apiPath.Replace("{apiTag}", txtCameraApiTag.Text.Trim());//将API地址中的{apiTag}替换成真实传感器API标识


            var qry = SDK.GetCameraInfo(txtGatewayTag.Text.Trim(), txtCameraApiTag.Text.Trim(), txtToken.Text);

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

            String apiPath = this.txtOnOfflineAPI.Text.Replace("{gatewayTag}", result.Msg);//将API地址中的{gatewayTag}替换成真实设备标识

            var qry = SDK.GetGatewayOnOffLine(result.Msg, txtToken.Text);

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

            String apiPath = this.txtHistoryPagerOnOfflineAPI.Text.Replace("{gatewayTag}", result.Msg);//将API地址中的{gatewayTag}替换成真实设备标识

            GatewayOnOfflineHistoryQryParas query = new GatewayOnOfflineHistoryQryParas()
            {
                PageIndex = 1,
                PageSize = 20,
                StartDate = DateTime.Now.AddDays(-30).ToShortDateString(),
                EndDate = DateTime.Now.ToShortDateString()
            };
            var qry = SDK.GetHistoryPagerOnOffline(result.Msg, query, txtToken.Text);

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

            String apiPath = this.txtStatusAPI.Text.Replace("{gatewayTag}", result.Msg);//将API地址中的{gatewayTag}替换成真实设备标识

            var qry = SDK.GetGatewayStatus(result.Msg, txtToken.Text);

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

            String apiPath = this.txtNewestDatasAPI.Text.Replace("{gatewayTag}", result.Msg);//将API地址中的{gatewayTag}替换成真实设备标识

            var qry = SDK.GetNewestDatas(result.Msg, txtToken.Text);

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

            String apiPath = this.txtNewestDataAPI.Text;
            apiPath = apiPath.Replace("{gatewayTag}", txtTab2GatewayTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath = apiPath.Replace("{apiTag}", txtNewestDataApiTag.Text.Trim());//将API地址中的{apiTag}替换成真实传感器API标识

            var qry = SDK.GetSensorNewestData(txtTab2GatewayTag.Text.Trim(),txtNewestDataApiTag.Text.Trim(), txtToken.Text);

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

            String apiPath = this.txtHistoryDataAPI.Text;
            apiPath = apiPath.Replace("{gatewayTag}", txtTab2GatewayTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath = apiPath.Replace("{apiTag}", txtHistoryDataApiTag.Text.Trim());//将API地址中的{apiTag}替换成真实传感器API标识

            GatewayDeviceHistoryQryParas query = new GatewayDeviceHistoryQryParas()
            {
                Method = 2,
                TimeAgo = 24,
            };

            var qry = SDK.GetSensorHistoryData(txtTab2GatewayTag.Text.Trim(), txtHistoryDataApiTag.Text.Trim(), query, txtToken.Text);

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

            String apiPath = this.txtHistoryPagerDataAPI.Text;
            apiPath = apiPath.Replace("{gatewayTag}", txtTab2GatewayTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath = apiPath.Replace("{apiTag}", txtHistoryPagerDataApiTag.Text.Trim());//将API地址中的{apiTag}替换成真实传感器API标识

            GatewayDeviceHistoryPagerQryParas query = new GatewayDeviceHistoryPagerQryParas()
            {
                PageIndex = 1,
                PageSize = 20,
                StartDate = DateTime.Now.AddDays(-30).ToShortDateString(),
                EndDate = DateTime.Now.ToShortDateString(),
            };

            var qry = SDK.GetSensorHistoryPagerData(txtTab2GatewayTag.Text.Trim(), txtHistoryPagerDataApiTag.Text.Trim(), query, txtToken.Text);

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

            String apiPath = this.txtActuatorNewestDataAPI.Text;
            apiPath = apiPath.Replace("{gatewayTag}", txtTab2GatewayTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath = apiPath.Replace("{apiTag}", txtActuatorNewestDataApiTag.Text.Trim());//将API地址中的{apiTag}替换成真实传感器API标识

            var qry = SDK.GetActuatorNewestData(txtTab2GatewayTag.Text.Trim(), txtActuatorNewestDataApiTag.Text.Trim(), txtToken.Text);

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

            String apiPath = this.txtActuatorHistoryDataAPI.Text;
            apiPath = apiPath.Replace("{gatewayTag}", txtTab2GatewayTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath = apiPath.Replace("{apiTag}", txtActuatorHistoryDataApiTag.Text.Trim());//将API地址中的{apiTag}替换成真实传感器API标识

            GatewayDeviceHistoryQryParas query = new GatewayDeviceHistoryQryParas()
            {
                Method = 2,
                TimeAgo = 24,
            };

            var qry = SDK.GetActuatorHistoryData(txtTab2GatewayTag.Text.Trim(), txtActuatorHistoryDataApiTag.Text.Trim(), query, txtToken.Text);

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

            String apiPath = this.txtActuatorHistoryPagerDataAPI.Text;
            apiPath = apiPath.Replace("{gatewayTag}", txtTab2GatewayTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识
            apiPath = apiPath.Replace("{apiTag}", txtActuatorHistoryPagerDataApiTag.Text.Trim());//将API地址中的{apiTag}替换成真实传感器API标识

            GatewayDeviceHistoryPagerQryParas query = new GatewayDeviceHistoryPagerQryParas()
            {
                PageIndex = 1,
                PageSize = 20,
                StartDate = DateTime.Now.AddDays(-30).ToShortDateString(),
                EndDate = DateTime.Now.ToShortDateString(),
            };

            var qry = SDK.GetActuatorHistoryPagerData(txtTab2GatewayTag.Text.Trim(), txtActuatorHistoryPagerDataApiTag.Text.Trim(), query, txtToken.Text);

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

            String apiPath = this.txtProjectInfoAPI.Text;
            apiPath = apiPath.Replace("{tag}", txtProjectTag.Text.Trim());//将API地址中的{gatewayTag}替换成真实设备标识

            var qry = SDK.GetProjectInfo(txtProjectTag.Text.Trim(), txtToken.Text);

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

            var qry = SDK.Control(txtTab5GatewayTag.Text.Trim(), txtControlApiTag.Text.Trim(), txtControlData.Text.Trim(), txtToken.Text);

            Out(qry, apiPath);
        }

        #endregion
    }
}
