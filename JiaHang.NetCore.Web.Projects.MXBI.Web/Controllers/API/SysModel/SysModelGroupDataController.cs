using JiaHang.NetCore.Web.Projects.MXBI.BLL.SysModelGroupBLL;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.SysModelGroup.RequestModel;
using JiaHang.NetCore.Web.Projects.MXBI.Web.WebApiIdentityAuth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiaHang.NetCore.Web.Projects.MXBI.Web.Controllers.API.SysModel
{
    [Route("api/[controller]")]
    public class SysModelGroupDataController:ControllerBase
    {

        private readonly SysModelGroupBLL  sysModelGroupBLL;
        public SysModelGroupDataController(DataContext dataContext)
        {
            sysModelGroupBLL = new SysModelGroupBLL(dataContext);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody]SearchSysModelGroupModel model)
        {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }

            return sysModelGroupBLL.Select(model);

        }

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<FuncResult> Select(int id)
        {
            return await sysModelGroupBLL.Select(id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FuncResult> Add([FromBody] SysModelGroupModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await sysModelGroupBLL.Add(model, CurrentUser.Get().Id);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<FuncResult> Update(int id, [FromBody]SysModelGroupModel model)
        {
            FuncResult data = await sysModelGroupBLL.Update(id, model, CurrentUser.Get().Id);
            return data;

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<FuncResult> Delete([FromRoute]int id)
        {
            return await sysModelGroupBLL.Delete(id, CurrentUser.Get().Id);

        }

        [Route("BatchDelete")]
        [HttpDelete]
        public async Task<FuncResult> Delete(int[] ids)
        {
            return await sysModelGroupBLL.Delete(ids, CurrentUser.Get().Id);

        }
    }
}
