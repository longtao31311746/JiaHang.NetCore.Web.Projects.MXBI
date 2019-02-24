using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    [Table("DIM_STORE")]
    public class DimStore : BaseEntity
    {
        [Key]
        public int id { get; set; }
        public string stCode { get; set; }
        public string stRegion { get; set; }
        public string stCity { get; set; }
        public string stLocation { get; set; }
        public string stBrand { get; set; }
        public string stOM { get; set; }
        public string stDM { get; set; }
        public string stName { get; set; }
        public string stName_en { get; set; }
        public string stAddress { get; set; }
        public string stAddress_en { get; set; }
        public string stType { get; set; }
        public DateTime? stOpenDate { get; set; }
        public DateTime? stCloseDate { get; set; }
        public string stTel { get; set; }
        public string stFax { get; set; }
        public string stEmail { get; set; }
        public string stIP { get; set; }
        public int? stSpace { get; set; }
        public int? stSeat { get; set; }
        public string stBizTime { get; set; }
        public string stPOST { get; set; }
        public string stSM { get; set; }
        public string stSMTel { get; set; }
        public string stComments { get; set; }

    }
}
