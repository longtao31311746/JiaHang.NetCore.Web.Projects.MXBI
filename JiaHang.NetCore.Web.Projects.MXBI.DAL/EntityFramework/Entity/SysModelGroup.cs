using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    [Table("SYS_MODEL_GROUP")]
   public class SysModelGroup:BaseEntity    {

        [Key]
        public int Model_Group_Id { get; set; }

        /// <summary>
        /// 组别代码
        /// </summary>
        [StringLength(30)]
        public string Model_Group_Code { get; set; }

        /// <summary>
        /// 组别名称
        /// </summary>
        [StringLength(60)]
        public string Model_Group_Name { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public int Parent_Id { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        public int Sort_Flag { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enable_Flag { get; set; }

        /// <summary>
        /// 模块组别图片地址
        /// </summary>
        [StringLength(300)]
        public string Image_Url { get; set; }
        /// <summary>
        /// 组别归属(APD,BIEE)
        /// </summary>
        [StringLength(30)]
        public string Group_Belong { get; set; }

        /// <summary>
        /// 模块所属业务代码
        /// </summary>
        [StringLength(30)]
        public string Biz_Sys_Code { get; set; }
    }
}
