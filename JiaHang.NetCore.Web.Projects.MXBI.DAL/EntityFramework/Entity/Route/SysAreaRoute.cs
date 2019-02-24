using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    /// <summary>
    /// 功能模块分区路径
    /// </summary>
    [Table("SYS_AREA_ROUTE")]
    public class SysAreaRoute:BaseEntity
    {
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// 区域别名  【中文名】
        /// </summary>
        public string AreaPath { get; set; }


        /// <summary>
        /// 区域地址
        /// </summary>
        public string AreaAlias { get; set; }


       


    }
}
