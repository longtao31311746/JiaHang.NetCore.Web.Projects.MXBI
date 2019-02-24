using System;
using System.Linq;
using JiaHang.NetCore.Web.Projects.MXBI.BLL.Relation;
using JiaHang.NetCore.Web.Projects.MXBI.BLL.SysUserInfoService;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.Account.Request;
using JiaHang.NetCore.Web.Projects.MXBI.Web.WebApiIdentityAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.NetCore.Web.Projects.MXBI.Web.Controllers.API.Account
{
    /// <summary>
    ///用于用户 登录 / 注销  注册 / 重置密码 
    /// </summary>
    [Route("api/[controller]")]
    public class AccountDataController : ControllerBase
    {
        private readonly CredentialsManage credentialsManage;
        private readonly SysUserInfoBLL sysUserInfoService;
        private readonly CurrentUserRouteBLL currentUserRouteBLL;
        public AccountDataController(DataContext context, IMemoryCache cache)
        {
            credentialsManage = new CredentialsManage(cache);
            sysUserInfoService = new SysUserInfoBLL(context);
            currentUserRouteBLL = new CurrentUserRouteBLL(context);
        }
        //GetSysUserInfoFirst


        [Route("login")]
        [HttpPost]
        public FuncResult Login([FromBody]AccountLogin model)
        {
            if (model == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "请输入正确用户名及密码" };
            }
            FuncResult<SysUserInfo> result = sysUserInfoService.Login(model.AccountName, model.Password);

            if (!result.IsSuccess)
            {
                return new FuncResult() { IsSuccess = result.IsSuccess, Message = result.Message };
            }

        

            //写入到缓存中
            AccountModel account = new AccountModel
            {
                Id = result.Content.User_Id,
                UserName = result.Content.User_Name,
                UserAccount = result.Content.User_Account,
                Email = result.Content.User_Email,
                IsLock = result.Content.User_Is_Lock,
                MobileNo = result.Content.User_Mobile_No,
                UserIsLdap = result.Content.User_Is_Ldap
            };
            //获取用户资源
            var routes_data = currentUserRouteBLL.GetRoutes(account.Id,result.Content.User_Account=="admin");
            
            if (routes_data.ViewRoutes.Count <= 0 )
            {
                return new FuncResult() { IsSuccess = false, Message = "当前用户暂无任何页面权限" };
            }

            string token = credentialsManage.SetAccount(account, model.Remember);
            // 加当前用户所拥有的route 写入缓存
            credentialsManage.SetAccountRoute(routes_data.Routes, token);
            var defaultUrl = routes_data.ViewRoutes.First().Controllers.First().Methods.First().CompletePath;
            if (string.IsNullOrWhiteSpace(defaultUrl)) {
                defaultUrl = "/home";
            }
            //defaultUrl = "/" + defaultUrl;
            HttpContext.Response.Cookies.Append("token", token, new CookieOptions() { Expires = DateTime.Now.AddDays(10) });//将token储存到本地
            return new FuncResult() { IsSuccess = result.IsSuccess, Content =new { account, routes_data.ViewRoutes, defaultUrl }, Message = result.Message };
        }

        [HttpDelete]
        public void SingOut()
        {
            string token = HttpContext.Request.Cookies["token"];
            HttpContext.Response.Cookies.Delete("token");
            credentialsManage.DeleteAccount(token);
        }
    }
}
