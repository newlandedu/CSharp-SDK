using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDKSample
{
    /// <summary>
    /// 返回结果对象
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 返回状态
        /// </summary>
        public ResultStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// 返回的状态码
        /// </summary>
        public Int32 StatusCode
        {
            get;
            set;
        }

        /// <summary>
        /// 返回的消息
        /// </summary>
        public String Msg
        {
            get;
            set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Result()
        {
            this.Status = ResultStatus.Success;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Status">执行状态</param>
        public Result(ResultStatus Status)
        {
            this.Status = Status;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Status">执行状态</param>
        /// <param name="Msg">返回的消息</param>
        public Result(ResultStatus Status, String Msg)
            : this(Status)
        {
            this.Msg = Msg;
        }

        /// <summary>
        /// 设置成功结果
        /// </summary>
        public virtual Result SetSuccess()
        {
            this.Status = ResultStatus.Success;
            return this;
        }

        /// <summary>
        /// 设置失败结果
        /// </summary>
        /// <param name="msg">失败的原因</param>
        public virtual Result SetFailure(String msg)
        {
            this.Status = ResultStatus.Failure;
            this.Msg = msg;
            return this;
        }

        /// <summary>
        /// 设置异常结果
        /// </summary>
        public virtual Result SetException(String msg)
        {
            this.Status = ResultStatus.Exception;
            this.Msg = msg;
            return this;
        }

        /// <summary>
        /// 设置未知结果
        /// </summary>
        public virtual Result SetUnknown(String msg)
        {
            this.Status = ResultStatus.Unknown;
            this.Msg = msg;
            return this;
        }

        /// <summary>
        /// 获取返回结果
        /// </summary>
        public virtual bool IsSuccess()
        {
            return this.Status == ResultStatus.Success;
        }

        /// <summary>
        /// 将本对象属性复制到另一个target对象
        /// </summary>
        /// <param name="target"></param>
        public virtual Result CopyTo(Result target)
        {
            target.StatusCode = this.StatusCode;
            target.Status = this.Status;
            target.Msg = this.Msg;
            return target;
        }
    }


    /// <summary>
    /// 返回结果对象
    /// </summary>
    /// <typeparam name="T">返回结果对象</typeparam>
    public class ResultMsg<T> : Result, ICloneable
    {
        private T mResultObj;
        /// <summary>
        /// 返回对象
        /// </summary>
        public T ResultObj
        {
            get
            {
                if (null == mResultObj)
                    mResultObj = default(T);
                return mResultObj;
            }
            set
            {
                mResultObj = value;
            }
        }

        /// <summary>
        /// 当返回失败时,可以与Msg结合使用,Msg返回简要信息;
        /// 而ErrorObj可以返回详细的失败对象,例如返回IDictionary<key, msg>这样的强类型
        /// </summary>
        public object ErrorObj
        {
            get;
            set;
        }

        #region -- 构造函数 --

        /// <summary>
        /// 构造函数
        /// </summary>
        public ResultMsg()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Status">执行状态</param>
        /// <param name="Msg">返回的消息</param>
        public ResultMsg(ResultStatus Status, String Msg)
            : base(Status, Msg)
        {

        }

        /// <summary>
        /// 构造函数（已过时）
        /// 已过时：与ResultMsg(ResultStatus Status, String Msg)会产生歧义
        /// 建议用：ResultMsg(ResultStatus Status, String Msg, T ResultObj)
        /// </summary>
        /// <param name="Status">执行状态</param>
        /// <param name="ResultObj">返回对象</param>
        public ResultMsg(ResultStatus Status, T ResultObj)
            : base(Status, null)
        {
            this.ResultObj = ResultObj;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Status">执行状态</param>
        /// <param name="Msg">返回的消息</param>
        /// <param name="ResultObj">返回对象</param>
        public ResultMsg(ResultStatus Status, String Msg, T ResultObj)
            : base(Status, Msg)
        {
            this.ResultObj = ResultObj;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Res">执行状态</param>
        public ResultMsg(Result Res)
            : base(Res.Status, Res.Msg)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Status">执行状态</param>
        public ResultMsg(ResultStatus Status)
            : base(Status, null)
        {

        }

        #endregion

        #region -- 公开方法 --

        /// <summary>
        /// 设置成功结果
        /// </summary>
        /// <param name="ResultObj">返回的对象</param>
        public ResultMsg<T> SetSuccess(T ResultObj)
        {
            SetSuccess();
            this.ResultObj = ResultObj;
            return this;
        }

        /// <summary>
        /// 设置成功结果
        /// </summary>
        public new ResultMsg<T> SetSuccess()
        {
            this.Status = ResultStatus.Success;
            return this;
        }

        /// <summary>
        /// 设置失败结果
        /// </summary>
        /// <param name="msg">失败的原因</param>
        public new ResultMsg<T> SetFailure(String msg)
        {
            this.Status = ResultStatus.Failure;
            this.Msg = msg;
            return this;
        }

        /// <summary>
        /// 设置异常结果
        /// </summary>
        public new ResultMsg<T> SetException(String msg)
        {
            this.Status = ResultStatus.Exception;
            this.Msg = msg;
            return this;
        }

        /// <summary>
        /// 设置未知结果
        /// </summary>
        public new ResultMsg<T> SetUnknown(String msg)
        {
            this.Status = ResultStatus.Unknown;
            this.Msg = msg;
            return this;
        }

        /// <summary>
        /// 克隆
        /// </summary>
        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// 将本对象属性复制到另一个ResultMsg对象
        /// </summary>
        /// <param name="target"></param>
        public virtual ResultMsg<TT> CopyTo<TT>(ResultMsg<TT> target)
        {
            target.StatusCode = this.StatusCode;
            target.Status = this.Status;
            target.Msg = this.Msg;
            if (typeof(TT) is T)
            {
                target.ResultObj = (TT)(object)this.ResultObj;
                target.ErrorObj = this.ErrorObj;
            }
            return target;
        }

        #endregion


    }

    /// <summary>
    /// 返回结果对象
    /// </summary>
    public class ResultMsg : ResultMsg<String>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ResultMsg()
            : base()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Status">执行状态</param>
        /// <param name="Msg">返回的消息</param>
        public ResultMsg(ResultStatus Status)
            : base(Status, "", null)
        {

        }
        /// <summary>
        /// /构造函数
        /// </summary>
        /// <param name="Status">执行状态</param>
        /// <param name="Msg">返回的消息</param>
        public ResultMsg(ResultStatus Status, String Msg)
            : base(Status, Msg, null)
        {

        }
    }
}
