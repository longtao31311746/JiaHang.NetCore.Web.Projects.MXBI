using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.FactTradeInfoDay.RequestModel
{
    public class FactTradeInfoDayModel
    {
        //    /// <summary>
        //    /// 门店编号
        //    /// </summary>
        //    [StringLength(50)]
        //    public string store_Code { get; set; }
        //    /// <summary>
        //    /// 交易日期
        //    /// </summary>
        //    public DateTime trade_Date { get; set; }
        //    /// <summary>
        //    /// 营业额
        //    /// </summary>
        //    public int trade_Money { get; set; }
        //    public int TC { get; set; }
        //    public int COVER { get; set; }
        //    public int AC { get; set; }
        public int limit { get; set; }
        public int page { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string stRegion { get; set; }
        public string stCity { get; set; }
        public string stDM { get; set; }
    }
}
