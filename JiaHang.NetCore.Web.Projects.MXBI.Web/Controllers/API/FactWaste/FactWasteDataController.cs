﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.NetCore.Web.Projects.MXBI.BLL.FactWaste;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.FactWaste.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace JiaHang.NetCore.Web.Projects.MXBI.Web.Controllers.API.FactWaste
{
    [Route("api/[controller]")]
    public class FactWasteDataController : Controller
    {
        //获取BLL上下文
        private readonly FactWasteBLL factWasteBLL;
        //构造函数中添加EF上下文
        public  FactWasteDataController(DataContext dataContext)
        {
            factWasteBLL = new FactWasteBLL(dataContext);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody]FactWasteModel model)
        {
            //接受传递过来的参数处理.

            return factWasteBLL.Select(model);

        }

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<FuncResult> Select(string id)
        {
            return await factWasteBLL.Select(id);
        }

        [Route("AcquisitionCityData")]
        [HttpGet()]
        public async Task<FuncResult> AcquisitionCityData()
        {
            return await factWasteBLL.AcquisitionCityData();
        }

        [HttpGet]
        [Route("Export")]
        public IActionResult Export(DateTime? StartTime, DateTime? EndTime, string stRegion, string stCity, string stDM, string stStore)
        {
            var model = new FactWasteModel()
            {
                stCity = stCity,
                StartTime = StartTime,
                EndTime = EndTime,
                stDM = stDM,
                stRegion = stRegion,
                storeCode = stStore
            };
            var result = factWasteBLL.GetWasteListBytes(model);

            return File(result, "application/ms-excel", $"Waste.xls");

        }

    }
}
