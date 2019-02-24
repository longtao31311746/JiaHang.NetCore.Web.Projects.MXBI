using JiaHang.NetCore.Web.Projects.MXBI.Model.Enumerations.SysModule;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.Relation
{
    public class SysModuleUserRelationModel
    {
        public string Id { get; set; }

        /// <summary>
        /// 用户/用户组ID
        /// </summary>
        public string UserGroupId { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// ModuleUser userid=user表id
        /// ModuleUserGroup userid=user_group 表
        /// </summary>
        //public ModuleUserRelation ModuleUserRelation { get; set; }

        /// <summary>
        /// 模块ID
        /// </summary>
        public string ModuleId { get; set; }

        /// <summary>
        /// 当前用户拥有的权限类别
        /// </summary>
        public PermissionType PermissionType { get; set; }


    }
}
