using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiaHang.NetCore.Web.Projects.MXBI.Web.WebApiIdentityAuth
{
    public static class MiddlewareExtensions
    {
      
        public static IApplicationBuilder UseIdentityAuth(this IApplicationBuilder builder) {

            return builder.UseMiddleware<IdentityAuthMiddleware>();
        }

        //public static IApplicationBuilder UseExceptionHanding(this IApplicationBuilder builder)
        //{
        //    return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        //}

    }
}
