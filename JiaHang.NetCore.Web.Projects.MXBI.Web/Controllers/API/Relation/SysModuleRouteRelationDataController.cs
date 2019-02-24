using JiaHang.NetCore.Web.Projects.MXBI.BLL.Relation;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.Relation;
using JiaHang.NetCore.Web.Projects.MXBI.Web.WebApiIdentityAuth;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JiaHang.NetCore.Web.Projects.MXBI.Web.Controllers.API
{
    [Route("api/[controller]")]
    public class SysModuleRouteRelationDataController : ControllerBase
    {
        private readonly SysModuleRouteRelationBLL  sysModuleRouteRelationBLL;
        public SysModuleRouteRelationDataController(DataContext datacontext)
        {
            sysModuleRouteRelationBLL = new SysModuleRouteRelationBLL(datacontext);
        }

        [HttpGet]
        public async Task<FuncResult>  Get()
        {
            var data = await sysModuleRouteRelationBLL.Select();
            return data;
        }

        [Route("AddOrUpdate")]
        [HttpPost]
        public async Task<FuncResult> AddOrUpdate([FromBody] List<ModuleRouteRelationRequestModel> data)
        {
            return await sysModuleRouteRelationBLL.AddOrUpdate(data, CurrentUser.Get().Id);

        }


        /// <summary>
        /// 获取未绑定用户/用户组
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("NotBindRoute")]
        public FuncResult NotBindRoute()
        {

            return sysModuleRouteRelationBLL.NotBindRoute();
        }


        [HttpDelete("{id}")]
        public async Task<FuncResult> Delte(string Id)
        {
            return await sysModuleRouteRelationBLL.Delete(Id, CurrentUser.Get().Id);
        }
    }
}
