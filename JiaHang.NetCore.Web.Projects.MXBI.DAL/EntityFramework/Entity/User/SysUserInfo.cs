using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JiaHang.NetCore.Web.Projects.MXBI.Model.Enumerations.Sys_User;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    [Table("SYS_USER_INFO")]
    public class SysUserInfo : BaseEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Key]
        public int User_Id { get; set; }


        /// <summary>
        /// 登录帐号
        /// </summary>
        [StringLength(30)]
        public string User_Account { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [StringLength(50)]
        public string User_Name { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [StringLength(30)]
        public string User_Password { get; set; }

        /// <summary>
        /// 用户组织ID
        /// </summary>
        public int User_Org_Id { get; set; }

        /// <summary>
        /// 用户组别名称列表，用逗号分割
        /// </summary>
        public int User_Group_Names { get; set; }

        /// <summary>
        /// 用户电子邮件地址
        /// </summary>
        [StringLength(60)]
        public string User_Email { get; set; }

        /// <summary>
        /// 是否是ldap用户（是：y,否：n）
        /// </summary>
        public bool User_Is_Ldap { get; set; }


        /// <summary>
        /// 用户手机号码
        /// </summary>
        [StringLength(30)]
        public string User_Mobile_No { get; set; }

        /// <summary>
        /// 用户系统归属(管理员：adm,领导层：led,kds报表用户:kds)
        /// </summary>
        public UserOwerType User_Ower { get; set; }


        /// <summary>
        /// 用户默认语言(zh-cn)
        /// </summary>
        [StringLength(30)]
        public string Language_Code { get; set; }

        /// <summary>
        /// 是否锁定该用户（是：y,否：n）
        /// </summary>
        public bool User_Is_Lock { get; set; }

        /// <summary>
        /// 有效开始日期
        /// </summary>
        public DateTime Eff_Start_Date { get; set; }

        /// <summary>
        /// 有效结束日期
        /// </summary>
        public DateTime Eff_End_Date { get; set; }



    }
}
