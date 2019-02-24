using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.DishColor.RequestModel;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.NetCore.Web.Projects.MXBI.BLL
{
    public class DishColorBLL
    {
        private readonly DataContext _context;

        public DishColorBLL(DataContext context)
        {
            _context = context;
        }

        public FuncResult Getlist(DishColorModel model)
        {
            var data = GetData(model);
            return new FuncResult { IsSuccess = true, Content = new { data } };
        }

        private IEnumerable<DishColorResultModel> GetData(DishColorModel model)
        {
            var _start = model.StartTime != null ? 
                (DateTime)model.StartTime : Convert.ToDateTime((DateTime.Now.AddDays(-10)).ToShortDateString());

            var _end = model.EndTime != null ? 
                (DateTime)model.EndTime : Convert.ToDateTime((DateTime.Now.AddDays(-1)).ToShortDateString());

            //var _quy = (
            //        from dcolor in _context.DishColors
            //        join prod in _context.DimProducts on dcolor.PRODUCT_CODE equals prod.code
            //        select new { dcolor }
            //    ).GroupBy(_ => new { _.dcolor.STORE_CODE, _.dcolor.PRODUCT_CODE }).Select(_ => _.Count()).ToArray();

            // 查询并过滤门店
            var stcodes = _context.DimStore.Where(_ => 
                    (string.IsNullOrWhiteSpace(model.Brand) || model.Brand.Contains(_.stBrand))
                    && (string.IsNullOrWhiteSpace(model.Region) || model.Region.Contains(_.stRegion))
                    && (string.IsNullOrWhiteSpace(model.City) || model.City.Contains(_.stCity))
                    && (string.IsNullOrWhiteSpace(model.DM) || model.DM.Contains(_.stDM))
                    && (string.IsNullOrWhiteSpace(model.StoreCode) || model.StoreCode.Contains(_.stCode))
                )
                .Select(_ => _.stCode)
                .ToList();

            // 报表
            var query = (
                    from x in (
                        from dcolor in _context.DishColors                    
                        join prod in _context.DimProducts on dcolor.PRODUCT_CODE equals prod.code
                        where dcolor.TRADE_DATE >= _start && dcolor.TRADE_DATE <= _end
                        where stcodes.Count <= 0 || stcodes.Contains(dcolor.STORE_CODE)
                        select new { prod, dcolor }
                    )
                    group x by new { x.prod.dish_Color, x.prod.price } 
                    into g
                    select new DishColorResultModel
                    {
                        DishColorName = g.Key.dish_Color,
                        Price = g.Key.price ?? 0,
                        ProductCount = g.Count(),
                        Belt = g.Sum(_ => _.dcolor.BELT_AMOUNT),
                        Pos = g.Sum(_ => _.dcolor.POST_AMOUNT),
                        BeltPosDiff = g.Sum(_ => _.dcolor.DIFF_AMOUNT),
                        DiffMoney = g.Sum(_ => _.dcolor.DIFF_MONEY),
                    }
                )
                .ToList();

            var allProductCount = query.Sum(_ => _.ProductCount);
            var allPos = query.Sum(_ => _.Pos);

            foreach (var m in query)
            {
                m.AllProductCount = allProductCount;
                m.ProductPercent = Convert.ToDouble(m.ProductCount / Convert.ToDouble(allProductCount)) * 100;
                m.AllPos = allPos;
                m.PosPercent = Convert.ToDouble(m.Pos / allPos) * 100;
            }

            query.Add(new DishColorResultModel
            {
                DishColorName = "合计",
                Price = -1,
                ProductCount = allProductCount,
                ProductPercent = 100, // allProductCount / allProductCount
                Belt = query.Sum(_ => _.Belt),
                Pos = query.Sum(_ => _.Pos),
                PosPercent = 100,    // allPos / allPos
                BeltPosDiff = query.Sum(_ => _.BeltPosDiff),
                DiffMoney = query.Sum(_ => _.DiffMoney),
            });

            return query;
        }

        public byte[] ExcelExport(DishColorModel model)
        {
            var comlumHeadrs = new[] { "碟色", "单价", "产品数", "产品占比", "Belt售出数", "POS售出数", "POS售出数%", "差异数量", "差异金额" };

            var data = GetData(model);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                worksheet.Cells["A1:I1"].Merge = true;
                worksheet.Cells[1, 1].Value =
                    $"{(string.IsNullOrEmpty(model.Region) ? "全国 " : model.Region + " ")}{(model.StartTime?.ToString("yyyy-MM-dd"))}至{(model.EndTime?.ToString("yyyy-MM-dd"))}";
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                var i = 0;
                for (; i < comlumHeadrs.Length; i++)
                {
                    worksheet.Cells[2, i + 1].Value = comlumHeadrs[i];
                    worksheet.Cells[2, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                var fill1 = worksheet.Cells["A2:I2"].Style.Fill;
                fill1.PatternType = ExcelFillStyle.Solid;
                fill1.BackgroundColor.SetColor(Color.Yellow);

                i = 3;
                foreach (var obj in data)
                {
                    worksheet.Cells["A" + i].Value = obj.DishColorName;
                    worksheet.Cells["A" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["B" + i].Value = $"{(obj.Price < 0 ? "" : $"{obj.Price.ToString("N2")}")}";
                    worksheet.Cells["C" + i].Value = obj.ProductCount;
                    worksheet.Cells["D" + i].Value = $"{obj.ProductPercent.ToString("N2")}%";
                    worksheet.Cells["E" + i].Value = obj.Belt;
                    worksheet.Cells["F" + i].Value = obj.Pos;
                    worksheet.Cells["G" + i].Value = $"{obj.PosPercent.ToString("N2")}%";
                    worksheet.Cells["H" + i].Value = obj.BeltPosDiff;
                    worksheet.Cells["I" + i].Value = obj.DiffMoney.ToString("N2");

                    var cells = worksheet.Cells[$"B{i}:I{i}"];
                    cells.AutoFitColumns(12);
                    cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    i++;
                }

                return package.GetAsByteArray();
            }
        }
    }
}
