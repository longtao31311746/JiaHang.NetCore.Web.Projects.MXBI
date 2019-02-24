using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.FactWaste.RequestModel
{
   public  class FactWasteModel
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
    }
}
