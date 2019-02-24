using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework
{
    public class BaseEntity
    {

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool Delete_Flag { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime Delete_Time { get; set; }

        /// <summary>
        /// 删除者id
        /// </summary>
        public int Delete_By { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Creation_Date { get; set; }

        /// <summary>
        /// 建立人
        /// </summary>
        public int Created_By { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime Last_Update_Date { get; set; }


        /// <summary>
        /// 最后更新人
        /// </summary>
        public int Last_Updated_By { get; set; }

    }
}
