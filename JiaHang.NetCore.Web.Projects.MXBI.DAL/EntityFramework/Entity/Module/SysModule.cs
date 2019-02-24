using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{

    /// <summary>
    /// 系统模块  
    /// 一个模块可以有多个子模块 但只会有一个父模块
    /// 一对多关系
    /// 子模块自动继承父模块权限
    /// </summary>
    [Table("SYS_MODULE")]
   public class SysModule:BaseEntity
    {
        /// <summary>
        /// 模块id
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        [StringLength(30)]
        public string ModuleName { get; set; }


        /// <summary>
        /// 模块父级Id 
        /// 递归获取  
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 模块级别
        /// 级别递增加1
        /// </summary>
        public int Level { get; set; }
    }
}
