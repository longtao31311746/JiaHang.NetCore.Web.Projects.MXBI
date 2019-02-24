using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity
{
    /// <summary>
    /// 
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("FACT_DISH_COLOR")]
    public partial class DishColor
    {

        /// <summary>
        /// 
        /// </summary> 
        [System.ComponentModel.DataAnnotations.Key]
        //db column is AutoIncrement 
        public int ID { get; set; }

        /// <summary>
        /// 门店编号
        /// </summary> 
        [StringLength(50)]
        public string STORE_CODE { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary> 
        [StringLength(50)]
        public string PRODUCT_CODE { get; set; }

        /// <summary>
        /// 交易日期
        /// </summary> 
        public DateTime TRADE_DATE { get; set; }

        /// <summary>
        /// Belt售出数
        /// </summary> 
        public decimal BELT_AMOUNT { get; set; }

        /// <summary>
        /// POS售出数
        /// </summary> 
        public decimal POST_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal DIFF_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal DIFF_MONEY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime CREATION_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int CREATE_BY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? LAST_UPDATE_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int? LAST_UPDATED_BY { get; set; }

    }
}
