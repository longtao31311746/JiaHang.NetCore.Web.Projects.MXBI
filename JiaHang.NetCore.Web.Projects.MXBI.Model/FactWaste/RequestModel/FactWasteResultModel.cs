using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.FactWaste.RequestModel
{
   public class FactWasteResultModel
    {
        //public string storeCode { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string dish_Color { get; set; }
        public string category { get; set; }
        public decimal? price { get; set; }
        public decimal? cost { get; set; }
        //public DateTime wasteDate { get; set; }
        public int wasteAmountBelt1300 { get; set; }
        public int wasteAmountBelt1400 { get; set; }
        public int wasteAmountBelt1500 { get; set; }
        public int wasteAmountBelt1600 { get; set; }
        public int wasteAmountBelt1700 { get; set; }
        public int wasteAmountBelt1800 { get; set; }
        public int wasteAmountBelt1900 { get; set; }
        public int wasteAmountBelt2000 { get; set; }
        public int wasteAmountBelt2100 { get; set; }
        public int wasteAmountBelt2200 { get; set; }
        public int wasteAmountBeltall { get; set; }
        public int productionAmount { get; set; }//制作数
        public string wasteBeltPercent { get; set; }
        public int wasteAmountNonBeltAll { get; set; }
        public int wasteAmountAll { get; set; }
        public int wasteMoneyAll { get; set; }
        public int wasteCostAll { get; set; }
    }
}
