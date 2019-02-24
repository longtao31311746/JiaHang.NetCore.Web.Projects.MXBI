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
    public class SysModuleUserRelationDataController : ControllerBase
    {
        private readonly CurrentUserRouteBLL currentUserRouteBLL;
        private readonly SysModuleUserRelationBLL sysModuleUserRelationBLL;
        public SysModuleUserRelationDataController(DataContext datacontext)
        {
            sysModuleUserRelationBLL = new SysModuleUserRelationBLL(datacontext);
            currentUserRouteBLL = new CurrentUserRouteBLL(datacontext);
        }

        [HttpGet]
        public async Task<FuncResult> Get()
        {
            var data = await sysModuleUserRelationBLL.Select();
            return data;
        }

        [Route("AddOrUpdate")]
        [HttpPost]
        public async Task<FuncResult> AddOrUpdate([FromBody] List<SysModuleUserRelationModel> data)
        {
            return await sysModuleUserRelationBLL.AddOrUpdate(data, CurrentUser.Get().Id);

        }


        /// <summary>
        /// 获取未绑定用户/用户组
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("NotBindUsers/{moduleId}")]
        public FuncResult NotBindUsers(string moduleId)
        {

            return sysModuleUserRelationBLL.NotBindUsers(moduleId);
        }


        [HttpDelete("{id}")]
        public async Task<FuncResult> Delte(string Id)
        {
            return await sysModuleUserRelationBLL.Delte(Id, CurrentUser.Get().Id);
        }


        [Route("GetRoutes")]
        [HttpGet]
        public object GetRoutes()
        {
            return currentUserRouteBLL.GetRoutes(CurrentUser.Get().Id);
        }
    }
}
