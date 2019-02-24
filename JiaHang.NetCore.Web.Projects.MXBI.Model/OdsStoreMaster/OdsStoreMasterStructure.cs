using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.OdsStoreMaster
{
    public class OdsStoreMasterStructure
    {
        public string Id { get; set; }
        public string ModuleName { get; set; }
        public int ModuleLevel { get; set; }
        public List<OdsStoreMasterStructure> ListChilds { get; set; }
        public OdsStoreMasterStructure()
        {
            ListChilds = new List<OdsStoreMasterStructure>();
        }
    }
}
