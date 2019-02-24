using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model
{
    public class UserGroupStructure
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public List<UserGroupStructure> ListChilds { get; set; }
        public UserGroupStructure()
        {
            ListChilds = new List<UserGroupStructure>();
        }
    }

}
