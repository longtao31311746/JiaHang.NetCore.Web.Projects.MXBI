using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.DimStore.RequestModel
{
    public class SearchDimStoreModel
    {
        public int limit { get; set; }
        public int page { get; set; }
        public string StCode { get; set; }
        public string StName { get; set; }
    }
}
