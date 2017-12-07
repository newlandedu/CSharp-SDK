using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK
{
    /// <summary>
    /// 单页数据集，IList集合格式
    /// </summary>
    [Serializable]
    public class ListPagerSet<T> : PagerSet
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public ListPagerSet()
            : base()
        {
            PageSet = new List<T>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageSet"></param>
        public ListPagerSet(int pageIndex, int pageSize, int pageCount, int recordCount, IList<T> pageSet)
            : base(pageIndex, pageSize, pageCount, recordCount)
        {
            this.PageSet = pageSet;
        }

        /// <summary>
        /// 数据集
        /// </summary>
        public IList<T> PageSet { get; set; }


        /// <summary>
        ///  检测 DataSet 数据集是否为空;是空值，返回 false；不是返回 true
        /// </summary>
        /// <returns></returns>
        public override bool CheckedPageSet()
        {
            if (PageSet == null)
            {
                return false;
            }
            if (PageSet.Count < 1)
            {
                return false;
            }

            return true;
        }

        
    }
}
