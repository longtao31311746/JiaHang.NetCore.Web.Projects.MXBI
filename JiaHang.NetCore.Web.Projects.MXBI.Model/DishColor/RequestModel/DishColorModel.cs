using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.DishColor.RequestModel
{
    public class DishColorModel
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string Brand { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string DM { get; set; }
        /// <summary>
        /// 门店编号传参数
        /// </summary>
        public string StoreCode { get; set; }

    }
}
