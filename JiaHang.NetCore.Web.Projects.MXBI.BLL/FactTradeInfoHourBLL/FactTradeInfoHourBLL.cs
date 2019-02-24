using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.FactTradeInfoHour.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.NetCore.Web.Projects.MXBI.BLL.FactTradeInfoHourBLL
{
    public class FactTradeInfoHourBLL
    {
        private readonly DataContext _context;
        public FactTradeInfoHourBLL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(FactTradeInfoHourModel model)
        {

            //先按条件查主表的数据返回一个list集合
            var stcodes = _context.OdsStoreMasters.Where(b => (string.IsNullOrWhiteSpace(model.stCity) || b.Stcity.Contains(b.Stcity))
                           && (string.IsNullOrWhiteSpace(model.stRegion) || b.Stregion.Contains(model.stRegion))
                           && (string.IsNullOrWhiteSpace(model.stDM) || b.Stdm.Contains(model.stDM))).Select(e => e.Stcode).ToList();
            //再按条件查出数据
            var query2 = (from a in _context.FactTradeInfoHours
                          where a.TRADE_DATE.ToShortDateString() == model.tradeDate.ToShortDateString()
                          group a by new { a.TRADE_DATE, a.TRADE_TIME_END, a.AC, a.TC, a.COVER, a.TRADE_MONEY }
                       into g
                          select new
                          {
                              TradeDate = g.Key.TRADE_DATE,
                              TradeTimeEnd = g.Key.TRADE_TIME_END,
                              Ac = g.Key.AC,
                              Cover = g.Key.COVER,
                              Tc = g.Key.TC,
                              TradeMoney = g.Key.TRADE_MONEY

                          }).ToList();
            //取得数据先
            //var query = from a in _context.OdsStoreMasters
            //            join b in _context.FactTradeInfoHours
            //            on a.Stcode equals b.STORE_CODE
            //            //条件
            //            where
            //            (b.TRADE_DATE.ToShortDateString() == model.tradeDate.ToShortDateString())
            //            &&
            //            (string.IsNullOrWhiteSpace(model.stCity) || a.Stcity.Contains(a.Stcity))
            //             &&
            //          (string.IsNullOrWhiteSpace(model.stRegion) || a.Stregion.Contains(model.stRegion))
            //             &&
            //          (string.IsNullOrWhiteSpace(model.stDM) || a.Stdm.Contains(model.stDM))
            //            select new FacTradHourRawModel
            //            {
            //                TradeDate = b.TRADE_DATE,
            //                TradeTimeEnd = b.TRADE_TIME_END,
            //                Ac = b.AC,
            //                Cover = b.COVER,
            //                Tc = b.TC,
            //                TradeMoney = b.TRADE_MONEY

            //            };
            var total = 0;
            if (model.tradeDate != null)
            {
                total = query2.Where(e => e.TradeDate == model.tradeDate).Count();
            }
            //根据最后小时排序分组成小时数据。
            var data = query2.ToList().GroupBy(e => e.TradeTimeEnd).Select(e => new
            {
                TradeTimeEnd = e.First().TradeTimeEnd.ToString(),
                TradeMoneey = e.Sum(a => a.TradeMoney),
                TotalAc = e.Sum(a => a.TradeMoney) == 0 ? 0 : e.Sum(a => a.TradeMoney) / e.Sum(a => a.Cover) == 0 ? 0 : e.Sum(a => a.Cover),
                Cover = e.Sum(a => a.Cover),
                Tc = e.Sum(a => a.Tc),
            });


            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FuncResult> Select(int id)
        {
            var entity = await _context.FactTradeInfoHours.FindAsync(id);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }




        public class FacTradHourRawModel
        {
            public DateTime TradeDate { get; set; }
            public string TradeTimeEnd { get; set; }
            public decimal? Ac { get; set; }
            public decimal Cover { get; set; }
            public decimal Tc { get; set; }
            public decimal TradeMoney { get; set; }
            public string Stdm { get; set; }
            public string Stregion { get; set; }
            public string Stcity { get; set; }
        }
    }
}

