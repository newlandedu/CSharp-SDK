using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class LibPagerParameters 
    {
        #region -- 私有成员 --

        private int m_pageIndex;
        private int m_pageSize;

        #endregion

        #region -- 属性成员 --

        /// <summary>
        /// 页面索引
        /// </summary>
        public virtual int PageIndex
        {
            get
            {
                return this.m_pageIndex;
            }
            set
            {
                if (this.m_pageIndex != value)
                {
                    this.m_pageIndex = value;
                }
            }
        }

        /// <summary>
        /// 页面大小
        /// </summary>
        public virtual int PageSize
        {
            get
            {
                return this.m_pageSize;
            }
            set
            {
                if (this.m_pageSize != value)
                {
                    this.m_pageSize = value;
                }
            }
        }

        #endregion

        #region -- 构造函数 --

        /// <summary>
        /// 构造函数
        /// </summary>
        public LibPagerParameters()
        {
            this.m_pageIndex = 1;
            this.m_pageSize = 20;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pageIndex"></param>
        public LibPagerParameters(int pageIndex)
        {
            this.m_pageIndex = pageIndex;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public LibPagerParameters(int pageIndex, int pageSize)
            : this(pageIndex)
        {
            this.m_pageSize = pageSize;
        }

        #endregion
    }

    /// <summary>
    /// 分页参数
    /// </summary>
    public class LibPagerParameters<TQryEntity> : LibPagerParameters
    {
        private TQryEntity mQryEntity;

        /// <summary>
        /// 查询实体数据
        /// </summary>
        public TQryEntity QryEntity
        {
            get
            {
                return this.mQryEntity;
            }
            set
            {
                this.mQryEntity = value;
            }
        }

        #region -- 构造函数 --

        /// <summary>
        /// 构造函数
        /// </summary>
        public LibPagerParameters()
            :base()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pageIndex"></param>
        public LibPagerParameters(int pageIndex)
            : base(pageIndex)
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public LibPagerParameters(int pageIndex, int pageSize)
            : base(pageIndex, pageSize)
        {
            
        }

        #endregion
    }
}
