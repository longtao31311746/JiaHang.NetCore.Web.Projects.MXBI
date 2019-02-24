using System;
using System.Collections.Generic;
using System.Text;


namespace JiaHang.NetCore.Web.Projects.MXBI.Model
{
    public class SearchSysModuleModel
    {
        public int limit { get; set; }
        public int page { get; set; }
        /// <summary>
        /// 模块级别
        /// </summary>
        public int ModuleLevel { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }
    }
}
