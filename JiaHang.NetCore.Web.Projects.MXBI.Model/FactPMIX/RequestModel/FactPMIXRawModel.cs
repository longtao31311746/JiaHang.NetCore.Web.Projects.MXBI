using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.FactPMIX.RequestModel
{
    public class FactPMIXRawModel
    {
        //查询条件
        public string ProductCode { get; set; }
        public DateTime TradeDate { get; set; }
        public string ProductName { get; set; }
        public string Dish_Color { get; set; }
        public string Category { get; set; }
        public decimal? price { get; set; }
        public decimal? cost { get; set; }
        public decimal? TradeAmount { get; set; }
        public decimal? TradePercent { get; set; }
        public decimal? TradeMoney { get; set; }
        public decimal? TradeAmountBelt { get; set; }
        public decimal? TradePercentBelt { get; set; }
        public decimal? TradeMoneyBelt { get; set; }
        public decimal? TradeAmountAlacart { get; set; }
        public decimal? TradePercentAlacart { get; set; }
        public decimal? TradeMoneyAlacart { get; set; }
        public decimal? CostMoney { get; set; }
        public decimal? CostPercent { get; set; }

    }
}
