using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using JiaHang.NetCore.Web.Projects.MXBI.Model.Enumerations.Sys_User;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.SysUserInfo.RequestModel
{
    public class SysUserInfoModel
    {
        [StringLength(30)]
        public string UserAccount { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [StringLength(50)]
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [StringLength(30)]
        public string UserPassword { get; set; }

        /// <summary>
        /// 用户组织ID
        /// </summary>
        public int UserOrgId { get; set; }

        /// <summary>
        /// 用户组别名称列表，用逗号分割
        /// </summary>
        public int UserGroupNames { get; set; }

        /// <summary>
        /// 用户电子邮件地址
        /// </summary>
        [StringLength(60)]
        public string UserEmail { get; set; }

        /// <summary>
        /// 是否是ldap用户（是：y,否：n）
        /// </summary>
        public bool UserIsLdap { get; set; }


        /// <summary>
        /// 用户手机号码
        /// </summary>
        [StringLength(30)]
        public string UserMobileNo { get; set; }

        /// <summary>
        /// 用户系统归属(管理员：adm,领导层：led,kds报表用户:kds)
        /// </summary>     
        public UserOwerType UserOwer { get; set; }


        /// <summary>
        /// 用户默认语言(zh-cn)
        /// </summary>
        [StringLength(30)]
        public string LanguageCode { get; set; }

        /// <summary>
        /// 是否锁定该用户（是：y,否：n）
        /// </summary>
        public bool UserIsLock { get; set; }

        /// <summary>
        /// 有效开始日期
        /// </summary>
        public DateTime EffStartDate { get; set; }

        /// <summary>
        /// 有效结束日期
        /// </summary>
        public DateTime EffEndDate { get; set; }
    }
}
