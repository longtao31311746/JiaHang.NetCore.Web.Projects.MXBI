using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.FactServiceTime.RequestModel
{
    public class FactServiceTimeResultModel
    {
        public string storeCode { get; set; }
        public DateTime serviceDate { get; set; }
        public string serviceType { get; set; }
        public decimal money { get; set; }
        public int cover { get; set; }
        public decimal ac { get; set; }
        public TimeSpan serviceTime1 { get; set; }
        public TimeSpan serviceTime2 { get; set; }
        public TimeSpan serviceTime3 { get; set; }
        public TimeSpan serviceTimeAll { get; set; }
    }
}
