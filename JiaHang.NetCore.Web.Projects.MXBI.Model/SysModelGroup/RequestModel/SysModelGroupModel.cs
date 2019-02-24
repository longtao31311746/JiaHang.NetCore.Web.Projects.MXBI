using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.SysModelGroup.RequestModel
{
   public class SysModelGroupModel
    {
        /// <summary>
        /// 组别代码
        /// </summary>
        [StringLength(30)]
        public string ModelGroupCode { get; set; }

        /// <summary>
        /// 组别名称
        /// </summary>
        [StringLength(60)]
        public string ModelGroupName { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        public int SortFlag { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool EnableFlag { get; set; }

        /// <summary>
        /// 模块组别图片地址
        /// </summary>
        [StringLength(300)]
        public string ImageUrl { get; set; }
        /// <summary>
        /// 组别归属(APD,BIEE)
        /// </summary>
        [StringLength(30)]
        public string GroupBelong { get; set; }

        /// <summary>
        /// 模块所属业务代码
        /// </summary>
        [StringLength(30)]
        public string BizSysCode { get; set; }
    }
}
