using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    [Table("SYS_SYSTEM_INFO")]
   public class SysSystemInfo
    {
        [Key]
        public int System_Id{ get; set; }
        [StringLength(30)]
        public string System_Code { get; set; }

        [StringLength(60)]
        public string System_Name { get; set; }

        [StringLength(60)]
        public string System_Url { get; set; }
    }
}
