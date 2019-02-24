using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework
{
    [Table("ODS_STOREMASTER")]
    public class OdsStoreMaster
    {
        [Key]
        public string Stcode { get; set; }
        public string Stregion { get; set; }
        public string Stcity { get; set; }
        public string Stlocation { get; set; }
        public string Stbrand { get; set; }
        public string Stom { get; set; }
        public string Stdm { get; set; }
        public string Stname { get; set; }
        public string Stname_en { get; set; }
        public string Staddress { get; set; }
        public string Staddress_en { get; set; }
        public string Sttype { get; set; }
        public string Stopendate { get; set; }
        public string Stclosedate { get; set; }
        public string Sttel { get; set; }
        public string Stfax { get; set; }
        public string Stemail { get; set; }
        public string Stip { get; set; }
        public string Stspace { get; set; }
        public string Stseat { get; set; }
        public string Stbiztime { get; set; }
        public string Stpost { get; set; }
        public string Stsm { get; set; }
        public string Stsmtel { get; set; }
        public DateTime? Stcomments { get; set; }
        public DateTime? Ststatus { get; set; }
    }
}
