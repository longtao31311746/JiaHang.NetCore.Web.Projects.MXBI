using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    [Table("DIM_PRODUCT")]
    public class DimProduct
    {
        [Key]
        public string code { get; set; }
        public string name { get; set; }
        public decimal? price { get; set; }
        public decimal? cost { get; set; }
        public string dish_Color { get; set; }
        public string category { get; set; }

    }
}
