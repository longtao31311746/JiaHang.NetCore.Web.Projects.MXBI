using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.DishColor.RequestModel
{
    public class DishColorResultModel
    {
        /// <summary>
        /// 色碟
        /// </summary>
        public string DishColorName { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 该色碟的产品种类数
        /// </summary>
        public int ProductCount { get; set; }
        /// <summary>
        /// 所有产品种类数
        /// </summary>
        public int AllProductCount { get; set; }
        /// <summary>
        /// 产品占比 = 该色碟的产品种类数/所有产品种类数
        /// </summary>
        public double ProductPercent { get; set; }
        /// <summary>
        /// Belt 售出数
        /// </summary>
        public decimal Belt { get; set; }
        /// <summary>
        /// POS 售出数
        /// </summary>
        public decimal Pos { get; set; }
        /// <summary>
        /// 所有色碟POS售出数量
        /// </summary>
        public decimal AllPos { get; set; }
        /// <summary>
        /// POS 售出数% 
        /// </summary>
        public double PosPercent { get; set; }
        /// <summary>
        /// 差异数量 = Belt 售出数 – POS 售出数
        /// </summary>
        public decimal BeltPosDiff { get; set; }
        /// <summary>
        /// 差异金额 = 差异数量 * 该色碟单价 
        /// </summary>
        public decimal DiffMoney { get; set; }
    }
}
