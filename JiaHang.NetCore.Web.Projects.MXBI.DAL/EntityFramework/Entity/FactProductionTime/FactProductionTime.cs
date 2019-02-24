using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    [Table("FACT_PRODUCTION_TIME")]
    public class FactProductionTime
    {
        public string store_Code { get; set; }
        [Key]
        public string product_Code { get; set; }
        public string product_Name { get; set; }
        public string product_Type { get; set; }
        public int product_Price { get; set; }
        public int product_Cost_Price { get; set; }
        public int order_Amount { get; set; }
        public DateTime production_Date { get; set; }
        public double production_Time { get; set; }
        public int production_Amount { get; set; }
        public int production_Money { get; set; }
        public decimal production_Money_Percent { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string production_Type { get; set; }
        public DateTime creation_Date { get; set; }
        public string create_By { get; set; }
        public DateTime last_Update_Date { get; set; }
        public string last_Updated_By { get; set; }

    }
}
