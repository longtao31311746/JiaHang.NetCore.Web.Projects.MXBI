using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    /// <summary>
    /// 模块-路径关联表
    /// 模块只控制到路径的权限
    /// 具体方法的权限控制在权限管控表上管理
    /// </summary>
    [Table("SYS_MODULE_ROUTE_RELATION")]
    public class SysModuleRouteRelation:BaseEntity
    {
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// 模块ID
        /// </summary>
        public string ModuleId { get; set; }

        /// <summary>
        /// 控制器路径
        /// </summary>
        public string ControllerRouteId { get; set; }

       
    }
}
