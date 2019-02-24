using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JiaHang.NetCore.Web.Projects.MXBI.BLL.FactTradeInfoDayBLL;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.FactTradeInfoDay.RequestModel;
using Microsoft.AspNetCore.Mvc;
using static JiaHang.NetCore.Web.Projects.MXBI.BLL.FactTradeInfoDayBLL.FactTradeInfoDayBLL;

namespace JiaHang.NetCore.Web.Projects.MXBI.Web.Controllers.API.FactTrade
{
    [Route("api/[controller]")]
    public class FactTradeInfoDataController : Controller
    {
        private readonly FactTradeInfoDayBLL FactTradeInfoDayBLL;
        public FactTradeInfoDataController(DataContext dataContext)
        {
            FactTradeInfoDayBLL = new FactTradeInfoDayBLL(dataContext);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody]FactTradeInfoDayModel model)
        {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }
            
            return FactTradeInfoDayBLL.Select(model);

        }

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<FuncResult> Select(int id)
        {
            return await FactTradeInfoDayBLL.Select(id);
        }

        [HttpGet]
        [Route("Export")]
        public  IActionResult Export(DateTime? StartTime, DateTime? EndTime,string stRegion,string stCity, string stDM)
        {
            var model = new FactTradeInfoDayModel()
            {
                stCity = stCity,
                StartTime = StartTime,
                EndTime = EndTime,
                stDM = stDM,
                stRegion = stRegion
            };
            var result =  FactTradeInfoDayBLL.GetTradeInfoDayListBytes(model);
            
            return File(result, "application/ms-excel", $"每日营业额.xls");

        }

        [Route("AcquisitionCityData")]
        [HttpGet()]
        public async Task<FuncResult> AcquisitionCityData() {
            return await FactTradeInfoDayBLL.AcquisitionCityData();
        }
    }
}