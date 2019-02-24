using JiaHang.NetCore.Web.Projects.MXBI.Model.Enumerations.SysModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    /// <summary>
    /// 资源具体路径
    /// </summary>
    [Table("SYS_METHOD_ROUTE")]
    public class SysMethodRoute:BaseEntity
    {
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// 控制器ID
        /// </summary>
        public string ControllerId { get; set; }

        /// <summary>
        /// 具体方法路径  
        /// 控制器后面所有路径
        /// </summary>
        public string MethodPath { get; set; }


        /// <summary>
        /// 方法别名
        /// </summary>
        public string MethodAlias { get; set; }

        /// <summary>
        /// 请求类型
        /// 先匹配类型 有多个类型相同，再匹配路径
        /// </summary>
        public string MethodType { get; set; }
    }
}
