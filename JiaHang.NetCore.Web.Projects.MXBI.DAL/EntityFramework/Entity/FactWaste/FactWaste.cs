using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    [Table("FACT_WASTE")]
    public class FactWaste
    {
        public string store_Code { get; set; }
        [Key]
        public string product_Code { get; set; }
        public string product_Name { get; set; }
        public string product_Dishcolor { get; set; }
        public string product_Type { get; set; }
        public decimal? product_Price { get; set; }
        public decimal? product_Cost_Price { get; set; }
        public DateTime waste_Date { get; set; }
        public int? waste_Amount_Belt_1300 { get; set; }
        public int? waste_Amount_Belt_1400 { get; set; }
        public int? waste_Amount_Belt_1500 { get; set; }
        public int? waste_Amount_Belt_1600 { get; set; }
        public int? waste_Amount_Belt_1700 { get; set; }
        public int? waste_Amount_Belt_1800 { get; set; }
        public int? waste_Amount_Belt_1900 { get; set; }
        public int? waste_Amount_Belt_2000 { get; set; }
        public int? waste_Amount_Belt_2100 { get; set; }
        public int? waste_Amount_Belt_2200 { get; set; }
        public int waste_Amount_Belt_all { get; set; }
        public int production_Amount { get; set; }
        public int? waste_Belt_Percent { get; set; }
        public int? waste_Amount_Non_Belt_All { get; set; }
        public int? waste_Amount_All { get; set; }
        public int? waste_Money_All { get; set; }
        public int? waste_Cost_All
        {
            get
            {
                return waste_Cost_All == null ? 0 : waste_Cost_All;
            }
            set {
                waste_Cost_All = value;
            }
        }
        public DateTime creation_Date { get; set; }
        public int create_By { get; set; }
        public DateTime last_Update_Date { get; set; }
        public int last_Updated_By { get; set; }
    }
}
