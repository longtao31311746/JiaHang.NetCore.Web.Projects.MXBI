using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity;
using JiaHang.NetCore.Web.Projects.MXBI.Model.Enumerations.Sys_User;
using System;
using System.ComponentModel.DataAnnotations;

namespace JiaHang.NetCore.Web.Projects.MXBI.Web.WebApiIdentityAuth
{

    public class AccountModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserAccount { get; set; }
        public string Email { get; set; }
        public bool UserIsLdap { get; set; }
        public string MobileNo { get; set; }
        public bool IsLock { get; set; }
    }
}
