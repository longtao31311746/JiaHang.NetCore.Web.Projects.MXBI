using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.FactServiceTime.RequestModel
{
   public class FactServiceTimeModel
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string stRegion { get; set; }
        public string stCity { get; set; }
        public string stDM { get; set; }

        /// <summary>
        /// 门店编号传参数
        /// </summary>
        [StringLength(50)]
        public string storeCode { get; set; }
        /// <summary>
        /// 用餐方式
        /// </summary>
        public string serviceType { get; set; }
    }
}
