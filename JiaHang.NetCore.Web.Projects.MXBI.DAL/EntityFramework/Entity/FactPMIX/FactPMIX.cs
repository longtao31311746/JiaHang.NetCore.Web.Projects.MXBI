using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    [Table("FACT_PMIX")]
    public class FactPMIX
    {
        public string store_Code { get; set; }
        [Key]
        public string product_Code { get; set; }

        public string product_Name { get; set; }

        public string product_Dishcolor { get; set; }

        public string product_Type { get; set; }

        public decimal? product_Price { get; set; }

        public decimal? product_Cost_Price { get; set; }

        public decimal trade_Price { get; set; }

        public DateTime trade_Date { get; set; }
        public decimal trade_Amount { get; set; }
        public decimal? trade_Percent { get; set; }
        public decimal trade_Money { get; set; }
        public decimal trade_Amount_Belt { get; set; }
        public decimal? trade_Percent_Belt { get; set; }
        public decimal trade_Money_Belt { get; set; }
        public decimal trade_Amount_Alacart { get; set; }
        public decimal? trade_Percent_Alacart { get; set; }
        public decimal trade_Money_Alacart { get; set; }
        public decimal cost_Money { get; set; }
        public decimal? cost_Percent { get; set; }
        public DateTime creation_Date { get; set; }
        public int create_By { get; set; }
        public DateTime last_Update_Date { get; set; }
        public int last_Updated_By { get; set; }
    }
}
