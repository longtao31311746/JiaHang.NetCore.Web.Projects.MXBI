using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.Enumerations.Sys_User
{
    /// <summary>
    /// 用户系统归属(管理员：adm, 领导层：led, kds报表用户:kds)
    /// </summary>
    public enum UserOwerType
    {
       
        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        Adm = 1,
        /// <summary>
        /// 领导层
        /// </summary>
        [Description("领导层")]
        Led = 2,
        /// <summary>
        /// kds报表用户
        /// </summary>
        [Description("kds报表用户")]
        Kds = 4,
    }
}
