using JiaHang.NetCore.Web.Projects.MXBI.BLL.FactServiceTime;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.FactServiceTime.RequestModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiaHang.NetCore.Web.Projects.MXBI.Web.Controllers.API.FactServiceTime
{
    [Route("api/[controller]")]
    public class FactServiceTimeDataController : Controller
    {

        private readonly FactServiceTimeBLL factServiceTimeBLL;
        public FactServiceTimeDataController(DataContext dataContext)
        {
            factServiceTimeBLL = new FactServiceTimeBLL(dataContext);
        }


        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] FactServiceTimeModel model)
        {
            return factServiceTimeBLL.Select(model);

        }

        public IActionResult Export(DateTime? StarTime, DateTime? EndTime, string stRegion, string stCity, string stDM, string stStore, string ServiceType)
        {
            var model = new FactServiceTimeModel()
            {
                stRegion = stRegion,
                stCity = stCity,
                stDM = stDM,
                storeCode = stStore,
                //数据类型 需判断
                serviceType = ServiceType,
                StartTime = StarTime,
                EndTime = EndTime
            };
            var result = factServiceTimeBLL.GetServiceTimeListBytes(model);
            return File(result, "application/ms-excel", $"ServiceTime.xls");
        }

        [Route("AcquisitionCityData")]
        [HttpGet()]
        public async Task<FuncResult> AcquisitionCityData()
        {
            return await factServiceTimeBLL.AcquisitionCityData();
        }
    }
}
