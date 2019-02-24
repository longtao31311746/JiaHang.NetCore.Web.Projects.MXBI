using System.Threading.Tasks;
using JiaHang.NetCore.Web.Projects.MXBI.BLL;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.SysRoute;
using JiaHang.NetCore.Web.Projects.MXBI.Web.WebApiIdentityAuth;
using Microsoft.AspNetCore.Mvc;

namespace JiaHang.NetCore.Web.Projects.MXBI.Web.Controllers.API.SysRoute
{
    [Route("api/[controller]")]
    public class SysRouteDataController : ControllerBase
    {
        private readonly SysRouteBLL sysRouteBLL;
        public SysRouteDataController(DataContext dataContext)
        {
            sysRouteBLL = new SysRouteBLL(dataContext);
        }

        [HttpGet]
        public async Task<FuncResult> Get()
        {
            return await sysRouteBLL.Select();
        }

        [Route("AddOrUpdateArea")]
        [HttpPost]
        public async Task<FuncResult> AddOrUpdateArea([FromBody]AreaRouteModel model)
        {
            return await sysRouteBLL.AddOrUpdateArea(model, CurrentUser.Get().Id);
        }


        [Route("AddOrUpdateController")]
        [HttpPost]
        public async Task<FuncResult> AddOrUpdateController([FromBody]ControllerRouteModel model)
        {
            return await sysRouteBLL.AddOrUpdateController(model, CurrentUser.Get().Id);
        }


        [Route("AddOrUpdateMethod")]
        [HttpPost]
        public async Task<FuncResult> AddOrUpdateMethod([FromBody]MethodRouteModel model)
        {
            return await sysRouteBLL.AddOrUpdateMethod(model, CurrentUser.Get().Id);
        }

        [HttpDelete]
        [Route("DeleteAreaRoute/{id}")]
        public async Task<FuncResult> DeleteAreaRoute(string id) {
            return await sysRouteBLL.DeleteAreaRoute(id, CurrentUser.Get().Id);
        }

        [HttpDelete]
        [Route("DeleteControllerRoute/{id}")]
        public async Task<FuncResult> DeleteControllerRoute(string id)
        {
            return await sysRouteBLL.DeleteControllerRoute(id, CurrentUser.Get().Id);
        }

        [HttpDelete]
        [Route("DeleteMethodRoute/{id}")]
        public async Task<FuncResult> DeleteMethodRoute(string id)
        {
            return await sysRouteBLL.DeleteMethodRoute(id, CurrentUser.Get().Id);
        }
    }
}
