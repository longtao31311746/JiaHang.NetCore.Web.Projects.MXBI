using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.BLL.OdsStoreMaster
{
    public class OdsStoreMasterBLL
    {
        private readonly DataContext _context;
        public OdsStoreMasterBLL(DataContext context)
        {
            _context = context;
        }
        //public FuncResult Select(string region)
        //{
        //    var query = _context.OdsStoreMasters;
            
        //}
    }
}
