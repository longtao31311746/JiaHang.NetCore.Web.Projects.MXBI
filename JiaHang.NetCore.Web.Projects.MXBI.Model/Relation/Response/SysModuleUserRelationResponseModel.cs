using JiaHang.NetCore.Web.Projects.MXBI.Model.Enumerations.SysModule;
using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.Relation.Response
{
    public class SysModuleUserRelationRawResponseModel
    {
        public string RelationId { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int ModuleLevel { get; set; }
        public string ModuleParentId { get; set; }
        public ModuleUserRelation ModuleUserRelation { get; set; }
        public PermissionType PermissionType { get; set; }
        public List<UserGroup> ListUserGroup { get; set; }
        public List<User> ListUser { get; set; }
    }

    public class SysModuleUserRelationResponseModel
    {
        
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int ModuleLevel { get; set; }
        public string ModuleParentId { get; set; }
        public ModuleUserRelation ModuleUserRelation { get; set; }
        public PermissionType PermissionType { get; set; }
        public List<UserGroup> ListUserGroup { get; set; }
        public List<User> ListUser { get; set; }
        public List<SysModuleUserRelationResponseModel> ListChildren { get; set; }
        public SysModuleUserRelationResponseModel() => ListChildren = new List<SysModuleUserRelationResponseModel>();
    }

    public class User
    {
        public string RelationId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserAccount { get; set; }
        public PermissionType PermissionType { get; set; }
    }

    public class UserGroup
    {
        public string RelationId { get; set; }
        public string UserGroupName { get; set; }
        public string UserGroupId { get; set; }
        public int UserGroupLevel { get; set; }
        public PermissionType PermissionType { get; set; }
    }
}
