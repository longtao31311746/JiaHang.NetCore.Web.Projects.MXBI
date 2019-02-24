using System.Threading.Tasks;
using JiaHang.NetCore.Web.Projects.MXBI.BLL.SysUserInfoService;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Web.WebApiIdentityAuth;
using JiaHang.NetCore.Web.Projects.MXBI.Model.SysUserInfo.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace JiaHang.NetCore.Web.Projects.MXBI.Web.Controllers.API.SysUser
{
    [Route("api/[controller]")]
    public class SysUserInfoDataController : ControllerBase
    {

        private readonly SysUserInfoBLL userInfoService;
        public SysUserInfoDataController(DataContext dataContext)
        {
            userInfoService = new SysUserInfoBLL(dataContext);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] SearchSysUserInfo model)
        {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }

            return userInfoService.Select(model);

        }

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<FuncResult> Select(int id)
        {
            return await userInfoService.Select(id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FuncResult> Add([FromBody] SysUserInfoModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await userInfoService.Add(model,CurrentUser.Get().Id);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<FuncResult> Update(int id, [FromBody]SysUserInfoModel model)
        {
            FuncResult data = await userInfoService.Update(id, model, CurrentUser.Get().Id);
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
            return await userInfoService.Delete(id, CurrentUser.Get().Id);

        }

        [Route("BatchDelete")]
        [HttpDelete]
        public async Task<FuncResult> Delete(int[] ids)
        {
            return await userInfoService.Delete(ids, CurrentUser.Get().Id);

        }

        [HttpGet]
        [Route("Export")]
        public async Task<IActionResult> Export() {
            var result =await  userInfoService.GetUserListBytes();
            return File(result, "application/ms-excel", $"系统用户.xlsx");
          
        }
    }
}