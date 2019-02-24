using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.FactProdutionTime.RequestModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Threading.Tasks;

namespace JiaHang.NetCore.Web.Projects.MXBI.BLL.FactProductionTime
{
    public class FactProductionTimeBLL
    {
        private readonly DataContext _dataContext;
        public FactProductionTimeBLL(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model">条件模型类</param>
        /// <returns></returns>
        public FuncResult Select(FactProductionTimeModel model)
        {
            var data = GetData(model);
            return new FuncResult() { IsSuccess = true, Content = new { data } };

        }
        /// <summary>
        /// 根据条件获得数据
        /// </summary>
        /// <param name="model">条件模型类</param>
        /// <returns>数据集合</returns>
        private IEnumerable<FactProductionTimeResultModel> GetData(FactProductionTimeModel model)
        {
            //首先获取产品表的带条件后的数据
            DateTime _start, _end;
            _start = model.StartTime != null ? (DateTime)
                model.StartTime : Convert.ToDateTime((DateTime.Now.AddDays(-10)).ToShortDateString());

            _end = model.EndTime != null ? (DateTime)
                model.EndTime : Convert.ToDateTime((DateTime.Now.AddDays(-1)).ToShortDateString());
            var stcodes = _dataContext.OdsStoreMasters.
                Where(b => (string.IsNullOrWhiteSpace(model.stRegion) || b.Stregion.Contains(model.stRegion))
            && (string.IsNullOrWhiteSpace(model.stCity) || b.Stcity.Contains(model.stCity))
            && (string.IsNullOrWhiteSpace(model.stDM) || b.Stdm.Contains(model.stDM))
            && (string.IsNullOrWhiteSpace(model.storeCode) || b.Stcode.Contains(model.storeCode))).Select(e => e.Stcode).ToList();            
            //类型条件
            //string sDataType = "";
            List<string> Types = new List<string> { "A-La-Cart", "AutoBelt", "Belt" };
            switch (model.dataType)
            {
                case 1:
                    //sDataType = "A-La-Cart";
                    Types = new List<string> { "A-La-Cart" };
                    break;
                case 2:
                    //仅belt数据
                    //sDataType = "AutoBelt,Belt";
                    Types = new List<string> { "AutoBelt", "Belt" };
                    break;
                case 3:
                    //sDataType = "Belt";
                    Types = new List<string> { "Belt" };
                    break;
            }

            //取关联表数据
            var query = (from a in _dataContext.FactProductionTimes
                         where a.production_Date >= _start && a.production_Date <= _end
                         && (stcodes.Count <= 0 || stcodes.Contains(a.store_Code))
                         //类型条件
                         && (Types.Count <= 0 || Types.Contains(a.production_Type))
                         group a by new
                         {
                             a.product_Code,
                             a.product_Name,
                             a.product_Type,
                             a.product_Price,
                         }
                      into g
                         select new
                         {
                             productCode = g.Key.product_Code,
                             product_Name = g.Key.product_Name,
                             product_Type = g.Key.product_Type,
                             product_Price = g.Key.product_Price,
                             //下单数量
                             order_Amount = g.Sum(su => su.order_Amount),
                             //制作总时间
                             production_Time = g.Sum(su => su.production_Time),
                             production_Amount = g.Sum(su => su.production_Amount),
                             production_Money = g.Sum(su => su.production_Money),
                         }).ToList();
            //取出所有产品金额
            int TotalproductionMoney = query.Sum(c => c.production_Money);
            int TotalproductionAmount = query.Sum(c => c.production_Amount);
            //总时间
            double TotalproductionTime = query.Sum(c => c.production_Time);
            //下单数量
            int TotalorderAmount = query.Sum(c => c.order_Amount);
            //平均时间
            double TotalproductionAvgTime = TotalorderAmount == 0 ? 0 : TotalproductionTime / TotalorderAmount;
            string sTotalproductionAvgTime = TotalproductionAvgTime.ToString("#0.00");
            var data = query.Select(a => new FactProductionTimeResultModel
            {
                productCode = a.productCode,
                productName = a.product_Name,
                productType = a.product_Type,
                productPrice = a.product_Price,
                //平均制作时间  =总时间除以下单数量
                productionAvgTime = (a.order_Amount == 0 ? 0 : a.production_Time / a.order_Amount).ToString("#0.00"),
                productionAmount = a.production_Amount,
                productionMoney = a.production_Money,
                //金额%计算公式：（该产品金额 /  所有产品金额）* 100。
                //TotalTradeMoney == 0 ? "0%" : (Convert.ToDecimal((a.ProductTradeMoney / TotalTradeMoney)) * 100).ToString("#0.00") + "%",
                moneyPercent = (Convert.ToDecimal(a.production_Money) / Convert.ToDecimal(TotalproductionMoney) * 100).ToString("#0.00") + "%"
            }).ToList();
            //增加一行汇总
            var addCount = new FactProductionTimeResultModel
            {
                productCode = "汇总：",
                productionAvgTime = sTotalproductionAvgTime,
                productionAmount = TotalproductionAmount,
                productionMoney = TotalproductionMoney,
            };
            //返回一个list集合
            data.Add(addCount);
            return data;
        }

        public byte[] GetProductionListBytes(FactProductionTimeModel model)
        {
            //二级表头数组
            var comlumHeadrs = new[] { "产品编号", "产品名称", "产品类别", "价格", "平均制作时间", "制作数量", "金额", "金额%" };
            byte[] result;
            //使用查询列表方法得到结果
            var data = GetData(model);
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1"); //Worksheet name
                                                                           //First add the headers               
                worksheet.Cells["A1:H1"].Merge = true;
                worksheet.Cells[1, 1].Value = "条件";
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

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
                    worksheet.Cells["C" + j].Value = obj.productType;
                    worksheet.Cells["D" + j].Value = obj.productPrice;
                    worksheet.Cells["E" + j].Value = obj.productionAvgTime;
                    worksheet.Cells["F" + j].Value = obj.productionAmount;//

                    worksheet.Cells["G" + j].Value = obj.productionMoney;
                    worksheet.Cells["H" + j].Value = obj.moneyPercent;

                    j++;
                }
                result = package.GetAsByteArray();
            }
            return result;
        }


        /// <summary>
        /// 获取city联动数据
        /// </summary>
        /// <returns></returns>
        public async Task<FuncResult> AcquisitionCityData()
        {
            var query = _dataContext.OdsStoreMasters.Select(e => new
            {
                Stregion = e.Stregion.Trim().ToLower(),
                Stcity = e.Stcity.Trim().ToLower(),
                Stdm = e.Stdm.Trim().ToLower(),
                Ststore = e.Stcode.Trim().ToLower()
            }).ToList();

            object data = null;
            await Task.Run(() =>
            {
                //品牌

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


    }
}

