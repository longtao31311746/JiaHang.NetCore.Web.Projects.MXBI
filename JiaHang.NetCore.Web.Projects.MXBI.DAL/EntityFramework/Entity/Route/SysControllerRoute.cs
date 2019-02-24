using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    [Table("SYS_CONTROLLER_ROUTE")]
   public class SysControllerRoute:BaseEntity
    {
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// 分区ID 若无不用填写
        /// </summary>
        public string AreaId { get; set; }
        /// <summary>
        /// 判断是否为api
        /// </summary>
        public bool IsApi { get; set; }

        /// <summary>
        /// 控制器路径
        /// </summary>
        public string ControllerPath { get; set; }


        /// <summary>
        /// 控制器别名
        /// </summary>
        public string ControllerAlias { get; set; }
    }
}
