using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.FactTradeInfoHour.RequestModel
{
    public class FactTradeInfoHourModel 
    {
        public DateTime tradeDate { get; set; }
        public string stRegion { get; set; }
        public string stCity { get; set; }
        public string stDM { get; set; }

    }
}