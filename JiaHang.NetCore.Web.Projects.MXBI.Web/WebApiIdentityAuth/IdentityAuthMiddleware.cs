using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using static JiaHang.NetCore.Web.Projects.MXBI.BLL.Relation.CurrentUserRouteBLL;

namespace JiaHang.NetCore.Web.Projects.MXBI.Web.WebApiIdentityAuth
{

    /// <summary>
    /// 身份验证中间件  
    /// </summary>
    public class IdentityAuthMiddleware
    {

        private readonly RequestDelegate _next;
        public IdentityAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, IMemoryCache cache)
        {
            //判断当前请求是否为静态文件
            string path = context.Request.Path.ToString().ToLower();
            string suffix = path.Split(".")[path.Split(".").Length - 1];
            if (suffix == "css" || suffix == "jpg" || suffix == "png")
            {
                await _next.Invoke(context);
                return;
            }

            if (path == "/account/login" || path == "/api/accountdata/login")
            {
                await _next.Invoke(context);
                return;
            }
            //验证当前请求是否存在token

            //验证当前用户
            IRequestCookieCollection cookies = context.Request.Cookies;
            string token = cookies["token"];
            if (string.IsNullOrEmpty(token))
            {
                context.Response.Redirect("/account/login");
                await Task.CompletedTask;
                return;
            }

            CredentialsManage credentialsManage = new CredentialsManage(cache);
            AccountModel account = credentialsManage.GetAccount(token);
            if (account == null)
            {
                context.Response.Redirect("/account/login");
                await Task.CompletedTask;
                return;
            }

            ///取出当前用户route信息 
            ///判断当前请求路径 是否有无权限
            List<BLL.Relation.CurrentUserRouteBLL.UserRouteRawModel> routes = credentialsManage.GetAccountRoute(token);
            var result= VerifyPath(routes, context.Request);
            //DateTime.Now.ToShortDateString();
            CurrentUser.Set(account);
            await _next.Invoke(context);
        }

        /// <summary>
        /// 验证路径
        /// </summary>
        /// <returns></returns>
        private bool VerifyPath(List<UserRouteRawModel> userRoutes, HttpRequest request)
        {
            var path = request.Path;
            //先判断有无该controller权限

            //再判断有无数据操作权限

            //即用户-模块绑定时所存储的权限 是否包含该路径的权限

            return false;
        }
    }

}
