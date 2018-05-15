using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLECloudSDK.Model
{
    /// <summary>
    /// 网关添加DTO
    /// </summary>
    public class GatewayAddApiDTO 
    {
       
        
        public virtual byte Kind { get; set; }


        public virtual string Name { get; set; }


        public virtual string Tag { get; set; }


       
        public virtual string Coordinate { get; set; }


      
        public virtual bool IsShare { get; set; }


        public virtual bool Enable { get; set; }
    }
}
