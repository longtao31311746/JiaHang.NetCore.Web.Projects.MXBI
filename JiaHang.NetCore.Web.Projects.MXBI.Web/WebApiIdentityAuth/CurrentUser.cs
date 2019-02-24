using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Web.WebApiIdentityAuth
{
    public class CurrentUser
    {
        // 第三种实现
         private static volatile AccountModel UserInfo;
        public static AccountModel Get() => UserInfo;        
        public static void Set(AccountModel model) => UserInfo = model;
        public static void Dispod() => UserInfo = null;

        //// 第二种实现
        // public static volatile AccountsModel UserInfo;

        //// 第一种实现
        // private volatile static AccountModel Instance;
        ////private readonly static object locker = new object();
        //public static void Set(AccountModel model)
        //{
        //    if (Instance == null)
        //    {
        //        Instance = new AccountModel()
        //        {
        //            Id = model.Id,
        //            Email = model.Email,
        //            NickName = model.NickName
        //        };
        //    }
        //}
        //public static AccountModel Get()
        //{
        //    return Instance;
        //}
        //public static void Dispod()
        //{
        //    Instance = null;
        //}
    }
}
