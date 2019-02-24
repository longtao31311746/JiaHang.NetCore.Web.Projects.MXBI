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
    public class SysUserGroupRelationDataController : ControllerBase
    {
        private readonly SysUserGroupRelationBLL sysUserGroupRelationBLL;
        public SysUserGroupRelationDataController(DataContext datacontext)
        {
            sysUserGroupRelationBLL = new SysUserGroupRelationBLL(datacontext);
        }

        [HttpGet]
        public FuncResult Get()
        {
            return sysUserGroupRelationBLL.Select();
        }
        [HttpGet]
        [Route("NotBindUser")]
        public FuncResult NotBindUser(string groupId)
        {
            return sysUserGroupRelationBLL.NotBindUser(groupId);
        }

        [HttpPost]
        public async Task<FuncResult> Post([FromBody]List<SysUserGroupRelationModel> model)
        {
            return await sysUserGroupRelationBLL.Add(model, CurrentUser.Get().Id);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">RelationId 绑定记录id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<FuncResult> Delete(string id)
        {
            return await sysUserGroupRelationBLL.Delete(id, CurrentUser.Get().Id);
        }

    }
}
