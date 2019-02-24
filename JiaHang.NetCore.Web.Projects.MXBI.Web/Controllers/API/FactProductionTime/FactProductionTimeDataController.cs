using JiaHang.NetCore.Web.Projects.MXBI.BLL.FactProductionTime;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.FactProdutionTime.RequestModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiaHang.NetCore.Web.Projects.MXBI.Web.Controllers.API.FactProductionTime
{
    [Route("api/[controller]")]
    public class FactProductionTimeDataController : Controller
    {
        //获取BLL上下文
        private readonly FactProductionTimeBLL factProductionTimeBLL;
        //构造函数中添加EF上下文
        public FactProductionTimeDataController(DataContext dataContext)
        {
            factProductionTimeBLL = new FactProductionTimeBLL(dataContext);
        }
        /// <summary>
        /// 查询列表方法
        /// </summary>
        /// <param name="model">body中的参数</param>
        /// <returns>list集合数据</returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] FactProductionTimeModel model)
        {
            return factProductionTimeBLL.Select(model);
        }
        /// <summary>
        /// 导出数据列表
        /// </summary>
        /// <param name="StarTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="stRegion"></param>
        /// <param name="stCity"></param>
        /// <param name="stDM"></param>
        /// <param name="stStore"></param>
        /// <param name="DataType"></param>
        /// <returns></returns>

        public IActionResult Export(DateTime? StarTime, DateTime? EndTime, string stRegion, string stCity, string stDM, string stStore, int DataType)
        {
            var model = new FactProductionTimeModel()
            {
                stRegion = stRegion,
                stCity = stCity,
                stDM = stDM,
                storeCode = stStore,
                //数据类型 需判断
                dataType = DataType,
                StartTime = StarTime,
                EndTime = EndTime
            };
            var result = factProductionTimeBLL.GetProductionListBytes(model);
            return File(result, "application/ms-excel", $"ProductionTime.xls");
        }

        [Route("AcquisitionCityData")]
        [HttpGet()]
        public async Task<FuncResult> AcquisitionCityData()
        {
            return await factProductionTimeBLL.AcquisitionCityData();
        }
    }
}
