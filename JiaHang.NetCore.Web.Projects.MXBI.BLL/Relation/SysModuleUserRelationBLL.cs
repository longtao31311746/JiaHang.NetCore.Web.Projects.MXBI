using System.Threading.Tasks;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.Relation;
using System.Linq;
using System.Collections.Generic;
using JiaHang.NetCore.Web.Projects.MXBI.Model.Relation.Response;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity;
using System;
using JiaHang.NetCore.Web.Projects.MXBI.Model.Enumerations.SysModule;

namespace JiaHang.NetCore.Web.Projects.MXBI.BLL.Relation
{
    /// <summary>
    /// 模块和用户绑定
    /// </summary>
    public class SysModuleUserRelationBLL
    {
        private readonly DataContext _context;
        public SysModuleUserRelationBLL(DataContext context)
        {
            _context = context;
        }



        /// <summary>
        /// 查询
        /// 获取接连的模块 所绑定的用户
        /// </summary>
        /// <returns></returns>
        public async Task<FuncResult> Select()
        {
            var query = from a in _context.SysModules
                        join b in _context.SysModuleUserRelations on a.Id equals b.ModuleId
                        into b_temp
                        from b_ifnull in b_temp.DefaultIfEmpty()// sysmodules与 sysmoduleUserRelation 的左连接
                        join c in _context.SysUserGroups on b_ifnull.UserGroupId equals c.Id
                        into c_temp
                        from c_ifnull in c_temp.DefaultIfEmpty()
                        join d in _context.SysUserInfos on b_ifnull.UserId equals d.User_Id
                        into d_temp
                        from d_ifnull in d_temp.DefaultIfEmpty()
                        orderby a.ModuleName
                        select new
                        {
                            RelationId = b_ifnull == null ? null : b_ifnull.Id,
                            ModuleId = a.Id,
                            a.ModuleName,
                            ModuleLevel = a.Level,
                            ModuleParentId = a.ParentId,
                            ModuleUserRelation = b_ifnull != null ? b_ifnull.ModuleUserRelation : 0,
                            PermissionType = b_ifnull != null ? b_ifnull.PermissionType : 0,

                            UserGroupIsNull = c_ifnull == null,
                            UserGroupName = c_ifnull != null ? c_ifnull.UserGroupName : null,
                            UserGroupId = c_ifnull != null ? c_ifnull.Id : null,
                            UserGroupLevel = c_ifnull != null ? c_ifnull.Level : 0,

                            UserIsNull = d_ifnull == null,
                            UserName = d_ifnull != null ? d_ifnull.User_Name : null,
                            UserId = d_ifnull != null ? d_ifnull.User_Id : 0,
                            UserAccount = d_ifnull != null ? d_ifnull.User_Account : null
                        };
            var data = query.ToList().GroupBy(e => e.ModuleId).Select(s => new SysModuleUserRelationRawResponseModel
            {

                ModuleId = s.Key,
                ModuleName = s.First().ModuleName,
                ModuleLevel = s.First().ModuleLevel,
                ModuleParentId = s.First().ModuleParentId,
                //ModuleUserRelation = s.First().ModuleUserRelation,

                ListUser = s.Where(user => !user.UserIsNull).Select(q => new User
                {
                    RelationId = q.RelationId,
                    UserId = q.UserId,
                    UserAccount = q.UserAccount,
                    UserName = q.UserName,
                    PermissionType = q.PermissionType


                }).ToList(),
                ListUserGroup = s.Where(group => !group.UserGroupIsNull).Select(q => new UserGroup
                {
                    RelationId = q.RelationId,
                    UserGroupId = q.UserGroupId,
                    UserGroupName = q.UserGroupName,
                    UserGroupLevel = q.UserGroupLevel,
                    PermissionType = q.PermissionType
                }).ToList()
            }).ToList();


            var result = await AcquisitionModule(data);
            //递归整理出树形结构

            return new FuncResult() { IsSuccess = true, Content = result };
        }


        public async Task<FuncResult> AddOrUpdate(List<SysModuleUserRelationModel> data, int currentUserId)
        {
            if (data.Count <= 0) return new FuncResult() { IsSuccess = false, Message = "请传递正确参数" };
            //不能重复绑定
            var list = _context.SysModuleUserRelations.Where(e => data.Select(m => m.ModuleId).Contains(e.ModuleId)).ToList();
            if (list.Count > 0)
            {
                if (list.Where(e => data.Where(c => string.IsNullOrWhiteSpace(c.Id)).Select(q => q.UserGroupId).Contains(e.UserGroupId)).Count() > 0)
                {
                    return new FuncResult() { IsSuccess = false, Message = "同一用户组不能重复绑定到同一模块上" };
                }
                if (list.Where(e => data.Where(c => string.IsNullOrWhiteSpace(c.Id)).Select(q => q.UserId).Contains(e.UserId)).Count() > 0)
                {
                    return new FuncResult() { IsSuccess = false, Message = "同一用户不能重复绑定到同一模块上" };
                }
            }

            //检查module 是否存在

            if (_context.SysModules.Count(q => data.Select(e => e.ModuleId).Contains(q.Id)) != data.Select(e => e.ModuleId).Distinct().Count())
            {
                return new FuncResult() { IsSuccess = false, Message = "模块id错误" };
            }
            //检查 userid/usergroupid是否存在
            if (_context.SysUserGroups.Count(q => data.Select(e => e.UserGroupId).Contains(q.Id)) != data.Select(e => e.UserGroupId).Distinct().Count(q => !string.IsNullOrWhiteSpace(q)))
            {
                return new FuncResult() { IsSuccess = false, Message = "用户组id错误" };
            }
            if (_context.SysUserInfos.Count(q => data.Select(e => e.UserId).Contains(q.User_Id)) != data.Select(e => e.UserId).Distinct().Count(q => q != 0))
            {
                return new FuncResult() { IsSuccess = false, Message = "用户id错误" };
            }

            // 检查更新模块与用户关联记录时  关联id是否正确

            if (_context.SysModuleUserRelations.Count(e => data.Select(c => c.Id).Contains(e.Id)) != data.Select(e => e.Id).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().Count())
            {
                return new FuncResult() { IsSuccess = false, Message = "更新模块与用户绑定记录时，未能根据关联id找到对应的关联记录" };
            }

            //添加
            var adds = data.Where(e => string.IsNullOrWhiteSpace(e.Id));
            var modifys = data.Where(e => !string.IsNullOrWhiteSpace(e.Id));
            await Task.Run(() =>
            {
                foreach (var add in adds)
                {
                    var add_entity = new SysModuleUserRelation()
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        ModuleId = add.ModuleId,
                        UserGroupId = add.UserGroupId,
                        UserId = add.UserId,
                        ModuleUserRelation = string.IsNullOrWhiteSpace(add.UserGroupId) ? ModuleUserRelation.ModuleUser : ModuleUserRelation.ModuleUserGroup,
                        PermissionType = add.PermissionType,

                        Creation_Date = DateTime.Now,
                        Created_By = currentUserId,
                        Last_Updated_By = currentUserId,
                        Last_Update_Date = DateTime.Now
                    };
                    _context.SysModuleUserRelations.Add(add_entity);
                }

                //更新
                foreach (var modify in modifys)
                {
                    var modify_enity = _context.SysModuleUserRelations.Find(modify.Id);

                    modify_enity.ModuleId = modify.ModuleId;
                    modify_enity.UserGroupId = modify_enity.UserGroupId;
                    modify_enity.UserId = modify_enity.UserId;
                    modify_enity.ModuleUserRelation = string.IsNullOrWhiteSpace(modify.UserGroupId) ? ModuleUserRelation.ModuleUser : ModuleUserRelation.ModuleUserGroup;
                    modify_enity.PermissionType = modify.PermissionType;
                    modify_enity.Last_Updated_By = currentUserId;
                    modify_enity.Last_Update_Date = DateTime.Now;
                    _context.SysModuleUserRelations.Update(modify_enity);
                }
            });


            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    LogService.WriteError(ex);
                    return new FuncResult() { IsSuccess = false, Message = "发生了预料之外的错误" };
                }
            }

            return new FuncResult() { IsSuccess = true };
        }

        /// <summary>
        /// 获取未绑定用户/用户组
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public FuncResult NotBindUsers(string moduleId)
        {
            var exists = _context.SysModuleUserRelations.Where(e => e.ModuleId == moduleId).ToList();

            //未绑定用户
            var users = _context.SysUserInfos.Where(e => !exists.Select(c => c.UserId).Contains(e.User_Id)).Select(q => new
            {
                UserId = q.User_Id,
                UserName = q.User_Name,
                UserAccount = q.User_Account
            });

            //未绑定用户组
            var userGroups = _context.SysUserGroups.Where(e => !exists.Select(c => c.UserGroupId).Contains(e.Id)).Select(q => new
            {
                UserGroupName = q.UserGroupName,
                UserGroupId = q.Id,
                UserGroupLevel = q.Level
            });
            return new FuncResult() { IsSuccess = true, Content = new { users, userGroups } };
        }

        public async Task<FuncResult> Delte(string id, int currentUserId)
        {
            var entity = await _context.SysModuleUserRelations.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "删除模块与用户绑定记录时，未能根据关联id找到对应的关联记录" };
            }
            entity.Delete_Flag = true;
            entity.Delete_Time = DateTime.Now;
            entity.Delete_By = currentUserId;

            _context.SysModuleUserRelations.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }

        #region 递归获取模块
        /// <summary>
        /// 递归获取模块
        /// </summary>
        /// <returns></returns>
        public async Task<List<SysModuleUserRelationResponseModel>> AcquisitionModule(List<SysModuleUserRelationRawResponseModel> data)
        {
            var parents = data.Where(e => string.IsNullOrWhiteSpace(e.ModuleParentId)).ToList();
            List<SysModuleUserRelationResponseModel> list = new List<SysModuleUserRelationResponseModel>();
            await Task.Run(() =>
            {
                foreach (var obj in parents)
                {
                    list.Add(Recursive(data, obj));
                }
            });
            return list;
        }

        /// <summary>
        /// 递归获取功能模块
        /// 并整理成树形结构
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private SysModuleUserRelationResponseModel Recursive(List<SysModuleUserRelationRawResponseModel> data, SysModuleUserRelationRawResponseModel parent)
        {

            var module = new SysModuleUserRelationResponseModel()
            {
                ModuleId = parent.ModuleId,
                ModuleLevel = parent.ModuleLevel,
                ModuleName = parent.ModuleName,
                ListUser = parent.ListUser,
                ListUserGroup = parent.ListUserGroup
            };
            //移除自身
            var childs = data.Where(e => e.ModuleParentId == parent.ModuleId).ToList();
            if (childs.Count == 0)
            {
                return module;
            }
            var parent_entity = data.FirstOrDefault(e => e.ModuleId == parent.ModuleId);
            if (parent_entity != null)
            {
                data.Remove(parent_entity);
            }
            foreach (var child in childs)
            {
                module.ListChildren.Add(Recursive(data.Where(e => e.ModuleParentId == child.ModuleId).ToList(), child));
            }
            return module;
        }

        #endregion
    
    }
}
