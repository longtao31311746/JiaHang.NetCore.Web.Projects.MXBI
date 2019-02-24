using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.BLL.Relation
{
    /// <summary>
    /// 获取当前用户资源目录
    /// </summary>
    public class CurrentUserRouteBLL
    {
        private readonly DataContext _context;
        public CurrentUserRouteBLL(DataContext dataContext)
        {
            _context = dataContext;
        }
        public CurrentUserRouteModel GetRoutes(int currentUserId, bool isAdmin = false)
        {
            //根据当前用户id 获取所有用户组

            List<SysModule> modules = new List<SysModule>();
            if (!isAdmin)
            {
                var query = (from a in _context.SysUserInfos
                             where a.User_Id == currentUserId
                             join b in _context.SysUserGroupRelations
                             on a.User_Id equals b.UserId
                             join c in _context.SysUserGroups
                             on b.UserGroupId equals c.Id
                             select c).ToList();
                var groups = new List<SysUserGroup>();
                var group_all_data = _context.SysUserGroups.ToList();
                foreach (var obj in query)
                {
                    groups.AddRange(RecursiveGetUserGroup(group_all_data, obj));
                }

              var  modules_query = (from a in _context.SysModules
                                 join b in _context.SysModuleUserRelations
                                 on a.Id equals b.ModuleId
                                 join c in groups on b.UserGroupId equals c.Id
                                 select a).ToList();
                var module_user = from a in _context.SysModules
                                  join b in _context.SysModuleUserRelations
                                  on a.Id equals b.ModuleId
                                  where b.UserId == currentUserId
                                  select a;
                modules_query.AddRange(module_user.ToList());
                var module_all_data = _context.SysModules.ToList();
                foreach (var obj in modules_query)
                {
                    modules.AddRange(RecursiveGetModule(module_all_data, obj));
                }
            }
            else
            {
                modules = _context.SysModules.ToList();
            }

            var routes = (from a in _context.SysControllerRoutes
                          join b in _context.SysModuleRouteRelations
                            on a.Id equals b.ControllerRouteId
                          join c in modules on b.ModuleId equals c.Id
                          join d in _context.SysMethodRoutes on a.Id equals d.ControllerId
                          join e in _context.SysAreaRoutes
                          on a.AreaId equals e.Id
                          into e_temp from e_ifnull in e_temp.DefaultIfEmpty()
                          orderby a.ControllerAlias
                          select new UserRouteRawModel
                          {
                              ControllerAlias = a.ControllerAlias,
                              ControllerPath = a.ControllerPath,
                              ControllerId = a.Id,
                              ControllerIsApi = a.IsApi,
                              MethodAlias = d.MethodAlias,
                              MethodPath = d.MethodPath,
                              MethodType = d.MethodType,
                              MethodId = d.Id,
                              ModuleId = c.Id,
                              ModuleName = c.ModuleName,
                              ModuleLevel = c.Level,
                              ModuleParentId = c.ParentId,
                             
                              AreaPath= e_ifnull==null?"": e_ifnull.AreaPath
                          }).ToList();

           
            var data = routes.Where(r=>!r.ControllerIsApi).GroupBy(e => e.ModuleId).Select(c => new ViewUserModuleResultModel
            {
                ModuleId = c.Key,
                ModuleName = c.First().ModuleName,
                ModuleLevel = c.First().ModuleLevel,
                ModuleParentId = c.First().ModuleParentId,             
                Controllers = c.GroupBy(q => q.ControllerId).Select(qc => new UserControllerResultModel
                {
                    ControllerId = qc.Key,
                    ControllerAlias = qc.First().ControllerAlias,
                    ControllerPath = qc.First().ControllerPath,
                    ControllerIsApi = qc.First().ControllerIsApi,
                    Methods = qc.GroupBy(qg => new { qg.ModuleId, qg.MethodAlias, qg.MethodPath, qg.MethodType }).Select(m => new UserMethodResultModel
                    {
                        MethodId = m.Key.ModuleId,
                        MethodAlias = m.Key.MethodAlias,
                        MethodPath = m.Key.MethodPath,
                        MethodType = m.Key.MethodType,
                        //CompletePath=string.IsNullOrWhiteSpace(m.First().AreaPath)? $"/{m.First().ControllerPath}/{m.First().MethodPath}":$"/{ m.First().AreaPath }"+ $"/{m.First().ControllerPath}/{m.First().MethodPath}"
                        CompletePath = (string.IsNullOrWhiteSpace(m.First().AreaPath)?"": $"/{ m.First().AreaPath }")+ $"/{m.First().ControllerPath}/{m.First().MethodPath}"
                    }).ToList()
                }).ToList()

            }).ToList();

            var result = GetRouteTree(data);
            return new CurrentUserRouteModel { Routes = routes, ViewRoutes = result };
        }

        public List<SysUserGroup> RecursiveGetUserGroup(List<SysUserGroup> data, SysUserGroup child)
        {
            //取出父级
            List<SysUserGroup> userGroups = new List<SysUserGroup>();
            userGroups.Add(child);

            //查看还有无父级
            var parents = data.Where(e => e.Id == child.ParentId).ToList();
            if (parents.Count <= 0)
            {
                return userGroups;
            }

            var child_entity = data.FirstOrDefault(e => e.Id == child.Id);
            if (child_entity != null)
            {
                data.Remove(child_entity);
            }
            foreach (var parent in parents)
            {
                userGroups.AddRange(RecursiveGetUserGroup(data, parent));
            }
            return userGroups;
        }
        public List<SysModule> RecursiveGetModule(List<SysModule> data, SysModule child)
        {
            //取出父级
            List<SysModule> moduleGroups = new List<SysModule>();
            moduleGroups.Add(child);

            //查看还有无父级
            var parents = data.Where(e => e.Id == child.ParentId).ToList();
            if (parents.Count <= 0)
            {
                return moduleGroups;
            }

            var child_entity = data.FirstOrDefault(e => e.Id == child.Id);
            if (child_entity != null)
            {
                data.Remove(child_entity);
            }
            foreach (var parent in parents)
            {
                moduleGroups.AddRange(RecursiveGetModule(data, parent));
            }
            return moduleGroups;
        }

        //整理成树形结构

        public List<ViewUserModuleResultModel> GetRouteTree(List<ViewUserModuleResultModel> data)
        {
            var result = new List<ViewUserModuleResultModel>();
            var parents = data.Where(e => string.IsNullOrWhiteSpace(e.ModuleParentId)).ToList();
            foreach (var obj in parents)
            {
                result.Add(GetUserRouteMoudleRecursive(data, obj));
            }

            return result;
        }


        public ViewUserModuleResultModel GetUserRouteMoudleRecursive(List<ViewUserModuleResultModel> data, ViewUserModuleResultModel parent)
        {

            //移除自身
            var childs = data.Where(e => e.ModuleParentId == parent.ModuleId).ToList();
            if (childs.Count == 0)
            {
                return parent;
            }
            var parent_entity = data.FirstOrDefault(e => e.ModuleParentId == parent.ModuleId);
            if (parent_entity != null)
            {
                data.Remove(parent_entity);
            }
            foreach (var child in childs)
            {
                parent.Children.Add(GetUserRouteMoudleRecursive(data.Where(e => e.ModuleParentId == child.ModuleId).ToList(), child));
            }
            return parent;
        }

        public class CurrentUserRouteModel
        {
            public List<ViewUserModuleResultModel> ViewRoutes { get; set; }
            public List<UserRouteRawModel> Routes { get; set; }
        }

        public class UserRouteRawModel
        {
            public string ControllerAlias { get; set; }
            public string ControllerPath { get; set; }
            public string ControllerId { get; set; }
            public bool ControllerIsApi { get; set; }
            public string MethodAlias { get; set; }
            public string MethodPath { get; set; }
            public string MethodType { get; set; }
            public string MethodId { get; set; }
            public string ModuleId { get; set; }
            public string ModuleName { get; set; }
            public int ModuleLevel { get; set; }
            public string ModuleParentId { get; set; }
            public string AreaPath { get; set; }
        }


        public class ViewUserModuleResultModel
        {           
            
            public string ModuleId { get; set; }
            public string ModuleName { get; set; }
            public int ModuleLevel { get; set; }
            public string ModuleParentId { get; set; }
            public List<UserControllerResultModel> Controllers { get; set; }
            public List<ViewUserModuleResultModel> Children { get; set; }

            public ViewUserModuleResultModel() {
                Children = new List<ViewUserModuleResultModel>();
                Controllers = new List<UserControllerResultModel>();
            }
        }


        public class UserControllerResultModel
        {
            public string ControllerAlias { get; set; }
            public string ControllerPath { get; set; }
            public string ControllerId { get; set; }
            public bool ControllerIsApi { get; set; }
            public List<UserMethodResultModel> Methods { get; set; }
        }

        public class UserMethodResultModel
        {
            public string MethodAlias { get; set; }
            public string MethodPath { get; set; }
            public string MethodType { get; set; }
            public string MethodId { get; set; }
            public string CompletePath { get; set; }
        }
    }
}
