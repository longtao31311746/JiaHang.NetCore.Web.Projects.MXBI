using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.FactProdutionTime.RequestModel
{
    public class FactProductionTimeResultModel
    {
        public string productCode { get; set; }
        public string productName { get; set; }
        public string productType { get; set; }
        public int productPrice { get; set; }
        public int productCostPrice { get; set; }
        public DateTime productionDate { get; set; }
        /// <summary>
        /// 平均时间
        /// </summary>
        public string productionAvgTime { get; set; }
        public int productionAmount { get; set; }
        public int productionMoney { get; set; }
        public string moneyPercent { get; set; }//金额百分比

    }
}
