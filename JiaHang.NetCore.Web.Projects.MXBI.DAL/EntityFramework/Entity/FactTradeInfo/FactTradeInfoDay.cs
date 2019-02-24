using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    [Table("FACT_TRADE_INFO_DAY")]
    public class FactTradeInfoDay
    {
        
        [StringLength(50)]
        public string STORE_CODE { get; set; }
        [Key]
        public DateTime TRADE_DATE { get; set; }
        public decimal TRADE_MONEY { get; set; }
        public decimal TC { get; set; }
        public decimal COVER { get; set; }
        public decimal? AC { get; set; }
    }
}
