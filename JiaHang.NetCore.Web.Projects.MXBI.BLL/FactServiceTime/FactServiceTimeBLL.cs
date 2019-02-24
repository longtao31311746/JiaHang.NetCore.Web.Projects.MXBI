using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.FactServiceTime.RequestModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.NetCore.Web.Projects.MXBI.BLL.FactServiceTime
{
    public class FactServiceTimeBLL
    {
        private readonly DataContext _dataContext;

        public FactServiceTimeBLL(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(FactServiceTimeModel model)
        {
            var data = GetData(model);
            return new FuncResult() { IsSuccess = true, Content = new { data } };
        }

        private IEnumerable<FactServiceTimeResultModel> GetData(FactServiceTimeModel model)
        {

            DateTime _start, _end;
            _start = model.StartTime != null ? (DateTime)model.StartTime : Convert.ToDateTime((DateTime.Now.AddDays(-10)).ToShortDateString());

            _end = model.EndTime != null ? (DateTime)model.EndTime : Convert.ToDateTime((DateTime.Now.AddDays(-1)).ToShortDateString());

            var stcodes = _dataContext.OdsStoreMasters
                .Where(b => (string.IsNullOrWhiteSpace(model.stRegion) || b.Stregion.Contains(model.stRegion))
                && (string.IsNullOrWhiteSpace(model.stCity)) || b.Stcity.Contains(model.stCity)
                && (string.IsNullOrWhiteSpace(model.stDM)) || b.Stdm.Contains(model.stDM)
                && (string.IsNullOrWhiteSpace(model.storeCode)) || b.Stcode.Contains(model.storeCode)).Select(e => e.Stcode).ToList();

            var query = (from a in _dataContext.FactServiceTimes
                         where a.service_Date >= _start && a.service_Date <= _end
                         && (stcodes.Count <= 0 || stcodes.Contains(a.store_Code))
                         && (string.IsNullOrWhiteSpace(model.serviceType) || a.service_Type.Contains(model.serviceType))
                         group a by new { a.service_Date } into g
                         select new
                         {
                             //临时数据
                             serviceDate = g.Key.service_Date,
                             money = g.Sum(su => su.money),
                             cover = g.Sum(su => su.cover),
                             ac = g.Sum(su => su.ac),
                             //开台到下单
                             // serviceTime1=g.Sum(su=>su.service_Time1),



                         }).ToList();

            var data = query.Select(a => new FactServiceTimeResultModel
            {
                serviceDate = a.serviceDate,
                money = a.money,
                cover = a.cover,
                ac = a.ac,
                //开台到下单
            }).ToList();



            return data;
        }

        public byte[] GetServiceTimeListBytes(FactServiceTimeModel model)
        {
            //
            var comlumHeadrs = new[] { "日期", "净营业额", "Cover", "AC", "开台到下单", "下单到上菜", "上菜到买单", "Stay Time" };

            byte[] result;
            //使用查询方法得到结果
            var data = GetData(model);
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1:H1"].Merge = true;
                worksheet.Cells[1, 1].Value = "条件";
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                for (int i = 0; i < comlumHeadrs.Count(); i++)
                {
                    worksheet.Cells[2, i + 1].Value = comlumHeadrs[i];
                    worksheet.Cells[2, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }


                var j = 3;//数据行
                foreach (var obj in data)
                {
                    var rt = obj.GetType();
                    var rp = rt.GetProperties();
                    worksheet.Cells["A" + j].Value = obj.serviceDate;
                    worksheet.Cells["A" + j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["B" + j].Value = obj.money;
                    worksheet.Cells["C" + j].Value = obj.cover;
                    worksheet.Cells["D" + j].Value = obj.ac;
                    worksheet.Cells["E" + j].Value = obj.serviceTime1;
                    worksheet.Cells["F" + j].Value = obj.serviceTime2;//

                    worksheet.Cells["G" + j].Value = obj.serviceTime3;
                    worksheet.Cells["H" + j].Value = obj.serviceTimeAll;
                    j++;
                }

                result = package.GetAsByteArray();
            }
            return result;
        }




        /// <summary>
        /// 异步获取city数据
        /// </summary>
        /// <returns></returns>

        public async Task<FuncResult> AcquisitionCityData()
        {
            var query = _dataContext.OdsStoreMasters.Select(e => new
            {
                Streion = e.Stregion.Trim().ToLower(),
                Stcity = e.Stcity.Trim().ToLower(),
                Stdm = e.Stdm.Trim().ToLower(),
                Ststore = e.Stcode.Trim().ToLower()
            }).ToList();
            object data = null;
            await Task.Run(() =>
            {
                //品牌
                //区域
                data = query.GroupBy(e => e.Streion).Select(c => new
                {
                    Streion = c.Key,
                    ListStcity = c.GroupBy(s => s.Stcity).Select(r => new
                    {
                        Stcity = r.Key,
                        ListStdm = r.GroupBy(w => w.Stdm).Select(m => new
                        {
                            Stdm = m.Key,
                            ListStore = m.GroupBy(z => z.Ststore).Select(u => new
                            {
                                Ststore = u.Key
                            })
                        })

                    })
                });
                //城市
                //经理
                //店铺

            });

            return new FuncResult() { IsSuccess = true, Content = data };
        }

    }
}
