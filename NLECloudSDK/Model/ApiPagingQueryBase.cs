using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// API分页查询基础模型
    /// </summary>
    public class ApiPagingQueryBase : LibPagerParameters
    {
        private Int32 mPageSize;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApiPagingQueryBase()
        {
            mPageSize = 20;
        }

        /// <summary>
        /// 指定每页要显示的数据个数，默认20，最多100
        /// </summary>
        public override int PageSize
        {
            get
            {
                return this.mPageSize;
            }
            set
            {
                if (this.mPageSize != value)
                {
                    mPageSize = value > 100 ? 100 : value;
                }
            }
        }

        /// <summary>
        /// 起始时间（可选，包括当天，格式YYYY-MM-DD）
        /// </summary>
        public virtual String StartDate { get; set; }

        /// <summary>
        /// 结束时间（可选，包括当天，格式YYYY-MM-DD）
        /// </summary>
        public virtual String EndDate { get; set; }
    }
}
