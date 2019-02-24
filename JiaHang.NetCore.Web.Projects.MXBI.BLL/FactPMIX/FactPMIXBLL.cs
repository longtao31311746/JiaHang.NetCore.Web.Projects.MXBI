using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.FactPMIX.RequestModel;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.NetCore.Web.Projects.MXBI.BLL
{
    public class FactPMIXBLL
    {
        private readonly DataContext _context;

        public FactPMIXBLL(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(FactPMIXModel model)
        {
            var data = GetData(model);
            return new FuncResult() { IsSuccess = true, Content = new { data } };

        }

        private IEnumerable<FactPMIXResultModel> GetData(FactPMIXModel model)
        {
            //total = 0;
            //根据页码取出  当前页要展示的交易记录
            DateTime _start, _end;
            _start = model.StartTime != null ? (DateTime)
                model.StartTime : Convert.ToDateTime((DateTime.Now.AddDays(-10)).ToShortDateString());

            _end = model.EndTime != null ? (DateTime)
                model.EndTime : Convert.ToDateTime((DateTime.Now.AddDays(-1)).ToShortDateString());

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //先去主表里面查店铺信息
            var stcodes = _context.OdsStoreMasters.Where(b => (string.IsNullOrWhiteSpace(model.stBrand) || b.Stbrand.Contains(model.stBrand))
                          && (string.IsNullOrWhiteSpace(model.stRegion) || b.Stregion.Contains(model.stRegion))
                          && (string.IsNullOrWhiteSpace(model.stCity) || b.Stcity.Contains(b.Stcity))
                          && (string.IsNullOrWhiteSpace(model.stDM) || b.Stdm.Contains(model.stDM))
                          && (string.IsNullOrWhiteSpace(model.storeCode) || b.Stcode.Contains(model.storeCode))).Select(e => e.Stcode).ToList();
            //取数据
            var query2 = (from a in _context.FactPMIXes
                          where a.trade_Date >= _start && a.trade_Date <= _end
                          && (stcodes.Count <= 0 || stcodes.Contains(a.store_Code))
                          group a by new { a.product_Code, a.product_Name, a.product_Dishcolor, a.product_Type, a.product_Price, a.product_Cost_Price }
                          //, a.trade_Price
                         into g
                          select new
                          {
                              productCode = g.Key.product_Code,
                              product_Name = g.Key.product_Name,
                              product_Dishcolor = g.Key.product_Dishcolor,
                              product_Type = g.Key.product_Type,
                              product_Price = g.Key.product_Price,
                              product_Cost_Price = g.Key.product_Cost_Price,


                              ProductTotalTradeAmount = g.Sum(su => su.trade_Amount),
                              ProductTotalTradeAmountBelt = g.Sum(su => su.trade_Amount_Belt),
                              ProductTotalTradeAmountAlacart = g.Sum(su => su.trade_Amount_Alacart),
                              ProductTradeMoney = g.Sum(su => su.trade_Money),
                              ProductTradeMoneyBelt = g.Sum(su => su.trade_Money_Belt),
                              ProductTradeMoneyAlacart = g.Sum(su => su.trade_Money_Alacart),
                              CostMoney = g.Sum(su => su.cost_Money),


                          }).ToList();
            decimal TotalTradeAmount = query2.Sum(c => c.ProductTotalTradeAmount);
            decimal TotalTradeAmountBelt = query2.Sum(c => c.ProductTotalTradeAmountBelt);
            decimal TotalTradeAmountAlacart = query2.Sum(c => c.ProductTotalTradeAmountAlacart);

            decimal TotalTradeMoney = query2.Sum(c => c.ProductTradeMoney);
            decimal TotalTradeMoneyBelt = query2.Sum(c => c.ProductTradeMoneyBelt);
            decimal TotalTradeMoneyAlacart = query2.Sum(c => c.ProductTradeMoneyAlacart);


            //var product_query = (from a in _context.DimProducts
            //                     join b in query2 on a.code equals b.productCode
            //                     select new
            //                     {
            //                         a.price,
            //                         a.cost,
            //                         a.name,
            //                         b.productCode,
            //                         b.ProductTotalTradeAmount,
            //                         b.ProductTotalTradeAmountAlacart,
            //                         b.ProductTotalTradeAmountBelt,

            //                         b.ProductTradeMoney,
            //                         b.ProductTradeMoneyBelt,
            //                         b.ProductTradeMoneyAlacart
            //                     }).ToList();
            var result = query2.Select(a => new FactPMIXResultModel
            {
                productCode = a.productCode,
                productName = a.product_Name,
                dish_Color = a.product_Dishcolor,
                category = a.product_Type,
                price = a.product_Price,
                cost = a.product_Cost_Price,

                tradeAmount = a.ProductTotalTradeAmount.ToString("N"),
                tradeAmountAlacart = a.ProductTotalTradeAmountAlacart,
                tradeAmountBelt = a.ProductTotalTradeAmountBelt,

                tradeMoney = a.ProductTradeMoney,
                tradeMoneyAlacart = a.ProductTradeMoneyAlacart,
                tradeMoneyBelt = a.ProductTradeMoneyBelt,

                tradePercent = TotalTradeAmount == 0 ? "0%" : (Convert.ToDecimal((a.ProductTotalTradeAmount / TotalTradeAmount)) * 100).ToString("#0.00") + "%",
                tradePercentBelt = TotalTradeAmountBelt == 0 ? "0%" : (Convert.ToDecimal((a.ProductTotalTradeAmountBelt / TotalTradeAmountBelt)) * 100).ToString("#0.00") + "%",
                tradePercentAlacart = TotalTradeAmountAlacart == 0 ? "0%" : (Convert.ToDecimal((a.ProductTotalTradeAmountAlacart / TotalTradeAmountAlacart)) * 100).ToString("#0.00") + "%",

                tradeMoneyPercent = TotalTradeMoney == 0 ? "0%" : (Convert.ToDecimal((a.ProductTradeMoney / TotalTradeMoney)) * 100).ToString("#0.00") + "%",

                costMoney = a.CostMoney,
                //【成本%】：计算公式：成本% =（该产品成本单价 /（产品销售单价/1.06））* 100

                costPercent = a.product_Price == 0 ? "0%" : Convert.ToDecimal(((a.product_Cost_Price / (a.product_Price / Convert.ToDecimal(1.06))) * 100)).ToString("#0.00") + "%"

            }).ToList();

            stopwatch.Stop();
            Console.WriteLine($"耗时{stopwatch.ElapsedMilliseconds}毫秒");


            var addCount = new FactPMIXResultModel
            {
                productCode = "汇总：",
                tradeAmount = TotalTradeAmount.ToString("N"),
                tradeMoney = TotalTradeMoney,
                tradeAmountBelt = TotalTradeAmountBelt,
                tradeMoneyBelt = TotalTradeMoneyBelt,
                tradeAmountAlacart = TotalTradeAmountAlacart,
                tradeMoneyAlacart = TotalTradeMoneyAlacart,
                //costMoney = ,
            };
            result.Add(addCount);
            return result;
        }

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FuncResult> Select(string id)
        {
            var entity = await _context.FactPMIXes.FindAsync(id);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }



        /// <summary>
        /// 获取city联动数据
        /// </summary>
        /// <returns></returns>
        public async Task<FuncResult> AcquisitionCityData()
        {
            var query = _context.OdsStoreMasters.Select(e => new
            {
                Stbrand = e.Stbrand.Trim().ToLower(),
                Stregion = e.Stregion.Trim().ToLower(),
                Stcity = e.Stcity.Trim().ToLower(),
                Stdm = e.Stdm.Trim().ToLower(),
                Ststore = e.Stcode.Trim().ToLower()
            }).ToList();

            object data = null;
            await Task.Run(() =>
            {
                //品牌
                data = query.GroupBy(e => e.Stbrand).Select(b => new
                {
                    Stbrand = b.Key,
                    //区域
                    ListStregion = b.GroupBy(e => e.Stregion).Select(c => new
                    {
                        Stregion = c.Key,
                        //城市
                        ListStcity = c.GroupBy(s => s.Stcity).Select(r => new
                        {
                            Stcity = r.Key,
                            //dm
                            ListStdm = r.GroupBy(w => w.Stdm).Select(m => new
                            {
                                Stdm = m.Key,
                                //店铺
                                ListStore = m.GroupBy(z => z.Ststore).Select(u => new
                                {
                                    Ststore = u.Key
                                })
                            })
                        })
                    })
                });
            });

            return new FuncResult() { IsSuccess = true, Content = data };
        }


        /// <summary>
        /// 导出按钮事件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public byte[] GetPMIXListBytes(FactPMIXModel model)
        {
            var comlumHeadrs = new[] { "产品编号", "产品名称", "色碟", "类别", "价格", "成本单价", "售出数", "售出数%", "金额", "金额%", "Belt售出", "Belt售出数%", "Belt金额", "A-La-Cart售出", "A-La-Cart售出数%", "A-La-Cart金额", "成本金额", "成本" };
            byte[] result;
            //使用查询得出结果
            var data = GetData(model);
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1"); //Worksheet name
                                                                           //First add the headers               
                worksheet.Cells["A1:J1"].Merge = true;
                worksheet.Cells[1, 1].Value = "条件";
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                worksheet.Cells["K1:M1"].Merge = true;
                worksheet.Cells[1, 11].Value = "Belt";
                worksheet.Cells[1, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                var fill1 = worksheet.Cells[1, 11].Style.Fill;
                fill1.PatternType = ExcelFillStyle.Solid;
                fill1.BackgroundColor.SetColor(Color.Yellow);
                worksheet.Cells["N1:P1"].Merge = true;
                worksheet.Cells[1, 14].Value = "A-La-Cart";
                worksheet.Cells[1, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                var fill = worksheet.Cells[1, 14].Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.Orange);
                worksheet.Cells["Q1:R1"].Merge = true;
                worksheet.Cells[1, 17].Value = "";
                worksheet.Cells[1, 17].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                for (var i = 0; i < comlumHeadrs.Count(); i++)
                {
                    worksheet.Cells[2, i + 1].Value = comlumHeadrs[i];
                    worksheet.Cells[2, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
                }

                //Add values
                var j = 3;
                // var chars = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                foreach (var obj in data)
                {
                    var rt = obj.GetType();
                    var rp = rt.GetProperties();

                    worksheet.Cells["A" + j].Value = obj.productCode;
                    //worksheet.Cells[1, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    worksheet.Cells["A" + j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["B" + j].Value = obj.productName;
                    worksheet.Cells["C" + j].Value = obj.dish_Color;
                    worksheet.Cells["D" + j].Value = obj.category;
                    worksheet.Cells["E" + j].Value = obj.price;
                    worksheet.Cells["F" + j].Value = obj.cost;//成本单价

                    worksheet.Cells["G" + j].Value = obj.tradeAmount;
                    worksheet.Cells["H" + j].Value = obj.tradePercent;
                    worksheet.Cells["I" + j].Value = obj.tradeMoney;
                    //售出金额
                    worksheet.Cells["J" + j].Value = obj.tradeAmountBelt;
                    worksheet.Cells["K" + j].Value = obj.tradeAmountBelt;
                    worksheet.Cells["L" + j].Value = obj.tradePercentBelt;

                    worksheet.Cells["M" + j].Value = obj.tradeMoneyBelt;
                    worksheet.Cells["N" + j].Value = obj.tradeAmountAlacart;
                    worksheet.Cells["O" + j].Value = obj.tradePercentAlacart;
                    worksheet.Cells["P" + j].Value = obj.tradeMoneyAlacart;
                    worksheet.Cells["Q" + j].Value = obj.costMoney;
                    worksheet.Cells["R" + j].Value = obj.costPercent;
                    j++;
                }
                result = package.GetAsByteArray();


            }
            return result;
        }

    }
}
