using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    /// <summary>
    /// 用户组
    /// 一个用户组可以有多个子用户组
    /// 子用户组 自动继承父用户组所拥有的权限
    /// </summary>
    [Table("SYS_USER_GROUP")]
    public class SysUserGroup:BaseEntity
    {
        [Key]
        public string Id { get; set; }

        public string ParentId { get; set; }
        public string UserGroupName { get; set; }
        public int Level { get; set; }
    }
}
