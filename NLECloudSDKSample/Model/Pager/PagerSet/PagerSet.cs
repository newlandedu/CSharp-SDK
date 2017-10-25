
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NLECloudSDKSample
{
    /// <summary>
    /// 单页数据集
    /// </summary>
    [Serializable]
    public abstract class PagerSet 
    {
        private int mPageCount;
        private int mPageIndex ;
        private int mPageSize ;
        private int mRecordCount ;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PagerSet()
        {
            this.mPageIndex = 1;
            this.mPageSize = 10;
            this.mPageCount = 0;
            this.mRecordCount = 0;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        public PagerSet(int pageIndex, int pageSize, int pageCount, int recordCount)
        {
            this.mPageIndex = pageIndex;
            this.mPageSize = pageSize;
            this.mPageCount = pageCount;
            this.mRecordCount = recordCount;
        }


        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount 
        {
            get 
            { 
                return mPageCount; 
            }
            set
            {
                mPageCount = value;
            }
        }

        /// <summary>
        /// 要显示的页码(页索引)
        /// </summary>
        public int PageIndex 
        {
            get
            {
                return mPageIndex;
            }
            set
            {
                mPageIndex = value;
            }
        }

        /// <summary>
        /// 每页的大小
        /// </summary>
        public int PageSize
        {
            get
            {
                return mPageSize;
            }
            set
            {
                mPageSize = value;
            }
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount
        {
            get
            {
                return mRecordCount;
            }
            set
            {
                mRecordCount = value;
            }
        }

        /// <summary>
        /// 检测页数据
        /// </summary>
        /// <returns></returns>
        public abstract bool CheckedPageSet();


        /// <summary>
        /// 将本对象属性复制到一个新对象
        /// </summary>
        /// <param name="target"></param>
        public virtual PagerSet CopyTo(PagerSet target)
        {
            if (null == target)
                return null;

            target.PageIndex = this.PageIndex;
            target.PageSize = this.PageSize;
            target.PageCount = this.PageCount;
            target.RecordCount = this.RecordCount;

            return target;
        }

    }
}
