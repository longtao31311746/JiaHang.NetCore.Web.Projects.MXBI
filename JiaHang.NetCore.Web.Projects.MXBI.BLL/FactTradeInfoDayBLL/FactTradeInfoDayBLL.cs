using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.FactTradeInfoDay.RequestModel;
using JiaHang.NetCore.Web.Projects.MXBI.Model.OdsStoreMaster;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.NetCore.Web.Projects.MXBI.BLL.FactTradeInfoDayBLL
{
    public class FactTradeInfoDayBLL
    {
        private readonly DataContext _context;
        public FactTradeInfoDayBLL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(FactTradeInfoDayModel model)
        {

            //var query = _context.FactTradeInfoDays;
            int total = 0;
            var data = GetData(model, out total);
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };

        }

        /// <summary>
        ///  获取数据
        /// </summary>
        /// <param name="model">传递参数</param>
        /// <param name="total">输出参数</param>
        /// <returns></returns>
        private IEnumerable<FacTradInfoResultModel> GetData(FactTradeInfoDayModel model, out int total)
        {
            total = 0;
            //根据页码取出  当前页要展示的交易记录
            DateTime _start, _end;
            //_start = model.StartTime != null ? (DateTime)
            //    model.StartTime : Convert.ToDateTime((DateTime.Now.AddDays(-model.page * model.limit - model.limit)).ToShortDateString());

            //_end = model.EndTime != null ? (DateTime)model.EndTime : Convert.ToDateTime((DateTime.Now.AddDays(-model.page * model.limit)).ToShortDateString());

            _start = model.StartTime != null ? model.StartTime.Value : DateTime.Now.AddMonths(-1);
            _end = model.EndTime != null ? model.EndTime.Value : DateTime.Now;


            //取得的/
            var query = from a in _context.OdsStoreMasters
                        join b in _context.FactTradeInfoDays
                        on a.Stcode equals b.STORE_CODE

                        where (string.IsNullOrWhiteSpace(model.stCity) || a.Stcity.Contains(a.Stcity))
                        && (string.IsNullOrWhiteSpace(model.stRegion) || a.Stregion.Contains(model.stRegion))
                        && (string.IsNullOrWhiteSpace(model.stDM) || a.Stdm.Contains(model.stDM))
                        && (b.TRADE_DATE >= _start && b.TRADE_DATE <= _end)
                        select new FacTradInfoRawModel
                        {
                            TradeDate = b.TRADE_DATE,
                            Ac = b.AC,
                            Cover = b.COVER,
                            Tc = b.TC,
                            TradeMoney = b.TRADE_MONEY

                        };
            total = 0;
            if (model.StartTime != null)
            {

                total = query.Where(e => e.TradeDate >= _start && e.TradeDate <= _end).
                    GroupBy(e => e.TradeDate.Day).Select(c => c.Key).Count();

            }
            else
            {
                total = query.GroupBy(e => e.TradeDate.Day).Select(c => c.Key).Count();
            }
            //
            #region 使用for循环来查询有空数据的日期。
            //var data = query.Where(e => e.TradeDate >= _start && e.TradeDate <= _end).ToList();
            //var result = new List<FacTradInfoResultModel>();
            //for (var i = 0; i < model.limit; i++) {
            //    var _c = _start.AddDays(i);
            //    var current_data = data.Where(y => y.TradeDate > _c && y.TradeDate <= _c.AddDays(1)).ToList();

            //    var _r = new FacTradInfoResultModel()
            //    {
            //        TradeDate = _c,
            //        //获取星期
            //        Week = getWeek(_c.DayOfWeek)
            //    };

            //    if (current_data.Count != 0)
            //    {
            //        _r.TotalAc = current_data.Sum(a => a.TradeMoney) / current_data.Sum(a => a.Cover);
            //        _r.TotalCover = current_data.Sum(a => a.Cover);
            //        _r.TotalTc = current_data.Sum(a => a.Tc);
            //        _r.TotalTradeMoney = current_data.Sum(a => a.TradeMoney);
            //    }

            //    result.Add(_r);
            //}
            #endregion


            var data = query.ToList().GroupBy(e => e.TradeDate.Day).Select(e => new FacTradInfoResultModel
            {
                TradeDate = e.First().TradeDate.ToShortDateString(),
                //获取星期
                Week = getWeek(e.First().TradeDate.DayOfWeek),
                TotalCover = e.Sum(a => a.Cover),
                TotalTc = e.Sum(a => a.Tc),
                TotalTradeMoney = e.Sum(a => a.TradeMoney),
                TotalAc = decimal.Parse((e.Sum(a => a.TradeMoney) / e.Sum(a => a.Cover)).ToString("#0.00")),
            }).ToList();
            //追加一行汇总行。
            decimal sTotalCover = 0, sTotalTc = 0, sTotalTradeMoney = 0, sTotalAc = 0;
            foreach (var obj in data)
            {
                sTotalCover += obj.TotalCover;
                sTotalTc += obj.TotalTc;
                sTotalTradeMoney += obj.TotalTradeMoney;
                sTotalAc = obj.TotalTradeMoney / obj.TotalCover;
            }
            var addCount = new FacTradInfoResultModel()
            {
                TradeDate = "汇总:",
                Week = "",
                TotalCover = sTotalCover,
                TotalTc = sTotalTc,
                TotalTradeMoney = sTotalTradeMoney,
                TotalAc = decimal.Parse(sTotalAc.ToString("#0.00")),
            };

            data.Add(addCount);
            return data;
        }

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FuncResult> Select(int id)
        {
            var entity = await _context.FactTradeInfoDays.FindAsync(id);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }
        /// <summary>
        /// 执行存储过程完成导出excel
        /// </summary>
        /// <returns></returns>
        //public async Task<byte[]> GetTeadeInfoDay()
        //{
        //    //按条件导出
        //    var comlumHeadrs = new[] { "序号",""};
        //    byte[] result;
        //    var data =await _context.ExecSpAsync<
        //    return result;
        //}

        public static string getWeek(DayOfWeek dt)
        {
            string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            string today_day = Day[Convert.ToInt16(dt)];
            return today_day;
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
                Stdm = e.Stdm.Trim().ToLower()
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
                            Stdm = m.Key
                        })
                    })
                });
            });

            return new FuncResult() { IsSuccess = true, Content = data };
        }



        public class FacTradInfoRawModel
        {
            public DateTime TradeDate { get; set; }
            public decimal? Ac { get; set; }
            public decimal Cover { get; set; }
            public decimal Tc { get; set; }
            public decimal TradeMoney { get; set; }
            public string Stdm { get; set; }
            public string Stregion { get; set; }
            public string Stcity { get; set; }
        }


        public class FacTradInfoResultModel
        {
            public string Week { get; set; }
            public string TradeDate { get; set; }
            public decimal? TotalAc { get; set; }
            public decimal TotalCover { get; set; }
            public decimal TotalTc { get; set; }
            public decimal TotalTradeMoney { get; set; }
            public string Stdm { get; set; }
            public string Stregion { get; set; }
            public string Stcity { get; set; }
        }

        /// <summary>
        /// 导出按钮事件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public byte[] GetTradeInfoDayListBytes(FactTradeInfoDayModel model)
        {
            var comlumHeadrs = new[] { "交易日期", "星期", "营业额", "TC", "COVER", "AC" };
            byte[] result;
            //使用查询得出结果
            int total = 0;
            var data = GetData(model, out total);
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1"); //Worksheet name
                //First add the headers
                for (var i = 0; i < comlumHeadrs.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                    worksheet.Cells[1, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
                }
                //Add values
                var j = 2;
                // var chars = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                foreach (var obj in data)
                {
                    var rt = obj.GetType();
                    var rp = rt.GetProperties();

                    worksheet.Cells["A" + j].Value = obj.TradeDate;
                    //worksheet.Cells[1, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    worksheet.Cells["A" + j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["B" + j].Value = obj.Week;
                    worksheet.Cells["C" + j].Value = obj.TotalTradeMoney;
                    worksheet.Cells["D" + j].Value = obj.TotalTc;
                    worksheet.Cells["E" + j].Value = obj.TotalCover;
                    worksheet.Cells["F" + j].Value = obj.TotalAc;
                    j++;
                }
                result = package.GetAsByteArray();


            }
            return result;
        }

    }


}
