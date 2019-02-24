using JiaHang.NetCore.Web.Projects.MXBI.BLL.FactTradeInfoHourBLL;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.FactTradeInfoHour.RequestModel;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiaHang.NetCore.Web.Projects.MXBI.Web.Controllers.API.FactTrade
{
    [Route("api/[controller]")]
    public class FactTradeHourDataController : Controller
    {
        private readonly FactTradeInfoHourBLL FactTradeHourBLL;
        public FactTradeHourDataController(DataContext dataContext)
        {
            FactTradeHourBLL = new FactTradeInfoHourBLL(dataContext);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody]FactTradeInfoHourModel model)
        {
            //model.page--; if (model.page < 0)
            //{
            //    model.page = 0;
            //}
            //接受传递过来的参数处理.

            return FactTradeHourBLL.Select(model);

        }

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<FuncResult> Select(int id)
        {
            return await FactTradeHourBLL.Select(id);
        }

        
    }
}