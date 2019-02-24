using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.FactPMIX.RequestModel
{
   public class FactPMIXResultModel
    {
        public string productCode { get; set; }
        public DateTime tradeDate { get; set; }
        public string productName { get; set; }
        public string dish_Color { get; set; }
        public string category { get; set; }
        public decimal? price { get; set; }
        public decimal? cost { get; set; }
        /// <summary>
        /// 售出数
        /// </summary>
        public string tradeAmount { get; set; }
        /// <summary>
        /// TRADE_PERCENT售出数%
        /// </summary>
        public string tradePercent { get; set; }
        public decimal? tradeMoney { get; set; }

        public string tradeMoneyPercent { get; set; }
        public decimal? tradeAmountBelt { get; set; }
        /// <summary>
        /// 售出数%(Belt)
        /// </summary>
        public string tradePercentBelt { get; set; }
        public decimal? tradeMoneyBelt { get; set; }
        public decimal? tradeAmountAlacart { get; set; }
        /// <summary>
        /// 售出数%(A-La-Cart)
        /// </summary>
        public string tradePercentAlacart { get; set; }
        public decimal? tradeMoneyAlacart { get; set; }
        public decimal? costMoney { get; set; }
        /// <summary>
        /// 成本%
        /// </summary>
        public string costPercent { get; set; }
    }
}
