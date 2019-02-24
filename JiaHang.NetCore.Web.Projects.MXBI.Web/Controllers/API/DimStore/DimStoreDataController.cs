using System.Threading.Tasks;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Web.WebApiIdentityAuth;
using JiaHang.NetCore.Web.Projects.MXBI.Model.DimStore.RequestModel;
using Microsoft.AspNetCore.Mvc;
using JiaHang.NetCore.Web.Projects.MXBI.BLL;

namespace JiaHang.NetCore.Web.Projects.MXBI.Web.Controllers.API.DimStore
{
    [Route("api/[controller]")]
    [ApiController]
    public class DimStoreDataController : ControllerBase
    {
        private readonly DimStoreBLL storeService;
        public DimStoreDataController(DataContext dataContext)
        {
            storeService = new DimStoreBLL(dataContext);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] SearchDimStoreModel model)
        {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }

            return storeService.Select(model);

        }

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<FuncResult> Select(int id)
        {
            return await storeService.Select(id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FuncResult> Add([FromBody] DimStoreModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await storeService.Add(model, CurrentUser.Get().Id);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="code"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<FuncResult> Update(int id, [FromBody]DimStoreModel model)
        {
            FuncResult data = await storeService.Update(id, model, CurrentUser.Get().Id);
            return data;

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<FuncResult> Delete([FromRoute]int id)
        {
            return await storeService.Delete(id, CurrentUser.Get().Id);

        }

        [Route("BatchDelete")]
        [HttpDelete]
        public async Task<FuncResult> Delete(int[] ids)
        {
            return await storeService.Delete(ids, CurrentUser.Get().Id);

        }
    }
}