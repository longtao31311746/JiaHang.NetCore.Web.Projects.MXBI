using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.SysRoute
{
   public class AreaRouteModel
    {

        public string Id { get; set; }

        /// <summary>
        /// 区域别名  【中文名】
        /// </summary>
        public string AreaPath { get; set; }


        /// <summary>
        /// 区域地址
        /// </summary>
        public string AreaAlias { get; set; }
    }
}
