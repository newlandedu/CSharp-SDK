using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// 单页数据集，DataSet格式
    /// </summary>
    [Serializable]
    public class DataSetPagerSet : PagerSet
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public DataSetPagerSet()
            : base()
        {
            this.PageSet = new DataSet("PagerSet");
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageSet"></param>
        public DataSetPagerSet(int pageIndex, int pageSize, int pageCount, int recordCount, DataSet pageSet)
            : base(pageIndex, pageSize, pageCount, recordCount)
        {
            this.PageSet = pageSet;
        }


        /// <summary>
        /// 数据集
        /// </summary>
        public DataSet PageSet { get; set; }


        /// <summary>
        ///  检测 DataSet 数据集是否为空;是空值，返回 false；不是返回 true
        /// </summary>
        /// <returns></returns>
        public override bool CheckedPageSet()
        {
            if (this.PageSet != null && this.PageSet.Tables.Count > 0)
                return true;
            else
                return false;
        }

       
    }
}
