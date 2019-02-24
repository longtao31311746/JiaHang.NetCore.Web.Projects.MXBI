using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    [Table("FACT_SERVICE_TIME")]
    public class FactServiceTime
    {
        public string store_Code { get; set; }
        [Key]
        public DateTime service_Date { get; set; }
        public string service_Type { get; set; }
        public decimal money { get; set; }
        public int cover { get; set; }
        public decimal ac { get; set; }
        public TimeSpan service_Time1 { get; set; }
        public TimeSpan service_Time2 { get; set; }
        public TimeSpan service_Time3 { get; set; }
        public TimeSpan service_Time_All { get; set; }
        public DateTime creation_Date { get; set; }
        public int create_By { get; set; }
        public DateTime last_Update_Date { get; set; }
        public int last_Updated_By { get; set; }
    }
}
