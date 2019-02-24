using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.FactProdutionTime.RequestModel
{
    public class FactProductionTimeModel
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
        /// 数据类型下拉条件1:A-La-Cart 2:  IN ('AutoBelt','Belt')" 3:Belt
        /// </summary>
        public int? dataType { get; set; }
    }
}
