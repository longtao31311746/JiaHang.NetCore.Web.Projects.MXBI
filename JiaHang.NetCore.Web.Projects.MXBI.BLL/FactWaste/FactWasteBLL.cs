using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.FactWaste.RequestModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace JiaHang.NetCore.Web.Projects.MXBI.BLL.FactWaste
{
    public class FactWasteBLL
    {
        private readonly DataContext _context;

        public FactWasteBLL(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(FactWasteModel model)
        {
            var data = GetData(model);
            return new FuncResult() { IsSuccess = true, Content = new { data } };

        }

        private IEnumerable<FactWasteResultModel> GetData(FactWasteModel model)
        {
            //total = 0;
            //根据页码取出  当前页要展示的交易记录
            DateTime _start, _end;
            _start = model.StartTime != null ? (DateTime)
                model.StartTime : Convert.ToDateTime((DateTime.Now.AddDays(-10)).ToShortDateString());

            _end = model.EndTime != null ? (DateTime)
                model.EndTime : Convert.ToDateTime((DateTime.Now.AddDays(-1)).ToShortDateString());



            var stcodes = _context.OdsStoreMasters.Where(b => (string.IsNullOrWhiteSpace(model.stCity) || b.Stcity.Contains(b.Stcity))
                         && (string.IsNullOrWhiteSpace(model.stRegion) || b.Stregion.Contains(model.stRegion))
                         && (string.IsNullOrWhiteSpace(model.stDM) || b.Stdm.Contains(model.stDM))
                         && (string.IsNullOrWhiteSpace(model.storeCode) || b.Stcode.Contains(model.storeCode))).Select(e => e.Stcode).ToList();


            var query = (from a in _context.FactWastes
                         where a.waste_Date >= _start && a.waste_Date <= _end && (stcodes.Count <= 0 || stcodes.Contains(a.store_Code))
                         group a by new { a.product_Code, a.product_Name, a.product_Dishcolor, a.product_Price, a.product_Type, a.product_Cost_Price }
                                     into g
                         select new
                         {
                             productCode = g.Key.product_Code,
                             productName = g.Key.product_Name,
                             productDishcolor = g.Key.product_Dishcolor,
                             productPrice = g.Key.product_Price,
                             productType = g.Key.product_Type,
                             productCostPrice = g.Key.product_Cost_Price==null?0:g.Key.product_Cost_Price,
                             ProductTotalWasteAmountBelt1300 = g.Sum(su => su.waste_Amount_Belt_1300),
                             ProductTotalWasteAmountBelt1400 = g.Sum(su => su.waste_Amount_Belt_1400),
                             ProductTotalWasteAmountBelt1500 = g.Sum(su => su.waste_Amount_Belt_1500),
                             ProductTotalWasteAmountBelt1600 = g.Sum(su => su.waste_Amount_Belt_1600),
                             ProductTotalWasteAmountBelt1700 = g.Sum(su => su.waste_Amount_Belt_1700),
                             ProductTotalWasteAmountBelt1800 = g.Sum(su => su.waste_Amount_Belt_1800),
                             ProductTotalWasteAmountBelt1900 = g.Sum(su => su.waste_Amount_Belt_1900),
                             ProductTotalWasteAmountBelt2000 = g.Sum(su => su.waste_Amount_Belt_2000),
                             ProductTotalWasteAmountBelt2100 = g.Sum(su => su.waste_Amount_Belt_2100),
                             ProductTotalWasteAmountBelt2200 = g.Sum(su => su.waste_Amount_Belt_2200),
                             ProductTotalWasteAmountBeltALL = g.Sum(su => su.waste_Amount_Belt_all),
                             ProductTotalProduction_Amount = g.Sum(su => su.production_Amount),
                             //waste%

                             ProductTotalWasteAmountNonBeltAll = g.Sum(su => su.waste_Amount_Non_Belt_All),
                             ProductTotalWasteAmountAll = g.Sum(su => su.waste_Amount_All),
                             ProductTotalWasteMoneyAll = g.Sum(su => su.waste_Money_All),
                             ProductTotalWasteCostAll = g.Sum(su => su.waste_Cost_All),
                         }).ToList();
            ////关联表
            //var product_query = (from a in _context.DimProducts
            //                     join b in query2 on a.code equals b.productCode
            //                     select new
            //                     {
            //                         a.price,
            //                         a.cost,
            //                         b.productCode,
            //                         a.name,
            //                         a.dish_Color,
            //                         a.category,
            //                         b.ProductTotalWasteAmountBelt1300,
            //                         b.ProductTotalWasteAmountBelt1400,
            //                         b.ProductTotalWasteAmountBelt1500,
            //                         b.ProductTotalWasteAmountBelt1600,
            //                         b.ProductTotalWasteAmountBelt1700,
            //                         b.ProductTotalWasteAmountBelt1800,
            //                         b.ProductTotalWasteAmountBelt1900,
            //                         b.ProductTotalWasteAmountBelt2000,
            //                         b.ProductTotalWasteAmountBelt2100,
            //                         b.ProductTotalWasteAmountBelt2200,
            //                         b.ProductTotalWasteAmountBeltALL,
            //                         b.ProductTotalProduction_Amount,
            //                         b.ProductTotalWasteAmountNonBeltAll,
            //                         b.ProductTotalWasteAmountAll,
            //                         b.ProductTotalWasteMoneyAll,
            //                         b.ProductTotalWasteCostAll
            //                     }).ToList();

            var result = query.Select(a => new FactWasteResultModel
            {
                price = a.productPrice,
                cost = a.productCostPrice==null?0 : a.productCostPrice,
                productName = a.productName,
                productCode = a.productCode,
                dish_Color = a.productDishcolor,
                category = a.productType,
                wasteAmountBelt1300 = (int)a.ProductTotalWasteAmountBelt1300,
                wasteAmountBelt1400 = (int)a.ProductTotalWasteAmountBelt1400,
                wasteAmountBelt1500 = (int)a.ProductTotalWasteAmountBelt1500,
                wasteAmountBelt1600 = (int)a.ProductTotalWasteAmountBelt1600,
                wasteAmountBelt1700 = (int)a.ProductTotalWasteAmountBelt1700,
                wasteAmountBelt1800 = (int)a.ProductTotalWasteAmountBelt1800,
                wasteAmountBelt1900 = (int)a.ProductTotalWasteAmountBelt1900,
                wasteAmountBelt2000 = (int)a.ProductTotalWasteAmountBelt2000,
                wasteAmountBelt2100 = (int)a.ProductTotalWasteAmountBelt2100,
                wasteAmountBelt2200 = (int)a.ProductTotalWasteAmountBelt2200,
                //Waste合计
                wasteAmountBeltall = a.ProductTotalWasteAmountBeltALL,
                //制作数
                productionAmount = a.ProductTotalProduction_Amount,
                //Waste%
                wasteBeltPercent = (a.ProductTotalProduction_Amount == 0 ? 0 :a.ProductTotalProduction_Amount / a.ProductTotalWasteAmountBeltALL).ToString("#0.00") + "%",
                //nonbelt
                wasteAmountNonBeltAll = (int)a.ProductTotalWasteAmountNonBeltAll,
                //数量
                wasteAmountAll = (int)a.ProductTotalWasteAmountAll,
                //金额
                wasteMoneyAll = (int)a.ProductTotalWasteMoneyAll,
                //成本
                wasteCostAll = a.ProductTotalWasteCostAll==null?0:(int)a.ProductTotalWasteCostAll,

            }).ToList();


            //合计
            var addCount = new FactWasteResultModel
            {
                productCode = "汇总：",


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
            var entity = await _context.FactWastes.FindAsync(id);

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
                Stregion = e.Stregion.Trim().ToLower(),
                Stcity = e.Stcity.Trim().ToLower(),
                Stdm = e.Stdm.Trim().ToLower(),
                Ststore = e.Stcode.Trim().ToLower()
            }).ToList();

            object data = null;
            await Task.Run(() =>
            {
                data = query.GroupBy(e => e.Stregion).Select(c => new
                {
                    Stregion = c.Key,
                    ListStcity = c.GroupBy(s => s.Stcity).Select(r => new
                    {
                        Stcity = r.Key,
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
                });
            });

            return new FuncResult() { IsSuccess = true, Content = data };
        }


        /// <summary>
        /// 导出按钮事件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public byte[] GetWasteListBytes(FactWasteModel model)
        {
            var comlumHeadrs = new[] { "产品编号", "产品名称", "产品类别", "单价", "成本价", "色碟", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "Waste合计", "制作数", "Waste%", "Watste数", "数量", "金额", "成本" };
            byte[] result;
            //使用查询得出结果
            var data = GetData(model);
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1"); //Worksheet name

                worksheet.Cells["A1:F1"].Merge = true;
                worksheet.Cells[1, 1].Value = "条件";
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                worksheet.Cells["G1:P1"].Merge = true;
                worksheet.Cells[1, 7].Value = "Belt Waste";
                worksheet.Cells[1, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                var fill1 = worksheet.Cells[1, 7].Style.Fill;
                fill1.PatternType = ExcelFillStyle.Solid;
                fill1.BackgroundColor.SetColor(Color.Yellow);
                worksheet.Cells["Q1:S1"].Merge = true;
                worksheet.Cells[1, 17].Value = "Belt";
                worksheet.Cells[1, 17].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                worksheet.Cells["T1:T1"].Merge = true;
                worksheet.Cells[1, 20].Value = "NonBelt";
                worksheet.Cells[1, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet.Cells["U1:W1"].Merge = true;
                worksheet.Cells[1, 21].Value = "总计Waste";
                worksheet.Cells[1, 21].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //First add the headers
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
                    worksheet.Cells["C" + j].Value = obj.category;
                    worksheet.Cells["D" + j].Value = obj.price;
                    worksheet.Cells["E" + j].Value = obj.cost;
                    worksheet.Cells["F" + j].Value = obj.dish_Color;//色碟

                    worksheet.Cells["G" + j].Value = obj.wasteAmountBelt1300;
                    worksheet.Cells["H" + j].Value = obj.wasteAmountBelt1400;
                    worksheet.Cells["I" + j].Value = obj.wasteAmountBelt1500;
                    worksheet.Cells["J" + j].Value = obj.wasteAmountBelt1600;
                    worksheet.Cells["K" + j].Value = obj.wasteAmountBelt1700;
                    worksheet.Cells["L" + j].Value = obj.wasteAmountBelt1800;

                    worksheet.Cells["M" + j].Value = obj.wasteAmountBelt1900;
                    worksheet.Cells["N" + j].Value = obj.wasteAmountBelt2000;
                    worksheet.Cells["O" + j].Value = obj.wasteAmountBelt2100;
                    worksheet.Cells["P" + j].Value = obj.wasteAmountBelt2200;
                    worksheet.Cells["Q" + j].Value = obj.wasteAmountBeltall;
                    worksheet.Cells["R" + j].Value = obj.productionAmount;
                    worksheet.Cells["S" + j].Value = obj.wasteBeltPercent;
                    worksheet.Cells["T" + j].Value = obj.wasteAmountNonBeltAll;
                    worksheet.Cells["U" + j].Value = obj.wasteAmountAll;
                    worksheet.Cells["V" + j].Value = obj.wasteMoneyAll;
                    worksheet.Cells["W" + j].Value = obj.wasteCostAll;
                    j++;
                }
                result = package.GetAsByteArray();


            }
            return result;
        }

    }
}

