using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    /// <summary>
    /// 权限管控
    /// 权限控制优先级高于 模块-用户表所设置的权限
    /// </summary>
    [Table("SYS_AUTHORITY_CONTROL")]
    public class SysAuthorityControl:BaseEntity
    {
        [Key]
        public string Id { get; set; }
        /// <summary>
        /// SysMethodRoute ID
        /// </summary>
        public string MethodId { get; set; }


        /// <summary>
        /// 是否允许
        /// </summary>
        public bool IsAllow { get; set; }
    }
}
