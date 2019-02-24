using JiaHang.NetCore.Web.Projects.MXBI.Model.Enumerations.SysModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    /// <summary>
    /// 用户组-模块关联表
    /// 用户可以绑定模块
    /// 用户组也可以绑定模块
    /// 用户可以有多个模块 
    /// 用户组也可以有多个模块
    /// 默认拥有控制器下所有方法权限 若需控制其不能访问 在权限控制表添加改method_id
    /// </summary>
    [Table("SYS_MODULE_USER_RELATION")]
    public class SysModuleUserRelation : BaseEntity
    {
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// 用户/用户组ID
        /// </summary>
        public string UserGroupId { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// ModuleUser userid=user表id
        /// ModuleUserGroup userid=user_group 表
        /// </summary>
        public ModuleUserRelation ModuleUserRelation { get; set; }

        /// <summary>
        /// 模块ID
        /// </summary>
        public string ModuleId { get; set; }

        /// <summary>
        /// 当前用户拥有的权限类别
        /// </summary>
        public PermissionType PermissionType { get; set; }
    }
}
