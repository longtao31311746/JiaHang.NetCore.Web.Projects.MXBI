using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JiaHang.NetCore.Web.Projects.MXBI.Model.Enumerations.SysModule;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    /// <summary>
    /// 用户与用户组的关联表
    /// 用户可以有多个用户组
    /// 组可以有多个用户
    /// 用户组内用户 自动继承用户组权限
    /// </summary>
    [Table("SYS_USER_GROUP_RELATION")]
    public class SysUserGroupRelation : BaseEntity
    {
        [Key]
        public string Id { get; set; }
        

        /// <summary>
        /// SysUserGroup 主键
        /// </summary>
        public string UserGroupId { get; set; }

        /// <summary>
        /// SysUserInfo主键
        /// </summary>
        public int UserId { get; set; }

    }
}
