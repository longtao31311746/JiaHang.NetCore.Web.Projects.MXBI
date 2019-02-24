using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.Account.Request
{
    public class AccountLogin
    {
        public string Password { get; set; }
        public string AccountName { get; set; }
        public bool Remember { get; set; }
        public string RedirectUrl { get; set; }
    }
}
