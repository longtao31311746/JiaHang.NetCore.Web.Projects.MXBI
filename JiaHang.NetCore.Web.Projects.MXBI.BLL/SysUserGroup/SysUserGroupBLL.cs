using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiaHang.NetCore.Web.Projects.MXBI.BLL
{
    public class SysUserGroupBLL
    {
        private readonly DataContext _context;
        public SysUserGroupBLL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(SearchSysUserGroupModel model)
        {
            var query = from a in _context.SysUserGroups
                        where (model.Level == 0 || a.Level == model.Level) && (string.IsNullOrWhiteSpace(model.Name) || a.UserGroupName.Contains(model.Name))
                        join b in _context.SysUserGroups on a.ParentId equals b.Id
                        into a_temp
                        from a_ifnull in a_temp.DefaultIfEmpty()
                        join c in _context.SysUserGroups on a_ifnull.ParentId equals c.Id
                        into b_temp
                        from c_ifnul in b_temp.DefaultIfEmpty()
                        orderby a.Creation_Date descending
                        select new
                        {
                            Id = a.Id,
                            Name = a.UserGroupName,
                            Level = ChineseCharacter(a.Level),
                            firstId = c_ifnul != null ? c_ifnul.Id : a_ifnull == null ? "" : a_ifnull.Id,
                            firstName = c_ifnul != null ? c_ifnul.UserGroupName : a_ifnull == null ? "未选择" : a_ifnull.UserGroupName,
                            //firstModuleName = a_ifnull != null ? a_ifnull.ModuleName : c_ifnul == null ? "未选择" : c_ifnul.ModuleName,
                            firstLevel = c_ifnul != null ? ChineseCharacter(c_ifnul.Level) : a_ifnull == null ? "" : ChineseCharacter(a_ifnull.Level),
                            secondId = c_ifnul == null ? "" : a_ifnull.Id,
                            secondName = c_ifnul == null ? "未选择" : a_ifnull.UserGroupName,
                            secondLevel = c_ifnul == null ? "" : ChineseCharacter(a_ifnull.Level),
                            Creation_Date = a.Creation_Date.ToString("yyyy-MM-dd HH:mm:ss")
                        };

            int total = query.Count();
            // var data = query.Skip(model.limit * model.page).Take(model.limit);
            var data = query.Skip(model.limit * model.page);//.Take(model.limit).ToList();

            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FuncResult> Select(string id)
        {
            var entity = await _context.SysUserGroups.FindAsync(id);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string id, UserGroupModel model, int currentuserId)
        {
            var level = 1;
            var entity = _context.SysUserGroups.FirstOrDefault(e => e.Id == id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "模块ID错误!" };
            }

            var parent = _context.SysUserGroups.FirstOrDefault(e => e.Id == model.ParentId);

            if (parent != null)
            {
                level = 2;
                if (_context.SysUserGroups.FirstOrDefault(e => e.Id == parent.ParentId) != null) {
                    level = 3;
                }
            }

            //模块名称不能重复
            if (_context.SysUserGroups.FirstOrDefault(e => e.UserGroupName == model.Name && e.Id != id) != null)
            {
                return new FuncResult() { IsSuccess = false, Message = "模块名不能重复!" };
            }


            entity.ParentId = model.ParentId;
            entity.UserGroupName = model.Name;
            entity.Level = level;
            entity.Last_Updated_By = currentuserId;
            entity.Last_Update_Date = DateTime.Now;


            _context.SysUserGroups.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        }
        public async Task<FuncResult> Delete(string id, int currentUserId)
        {
            var entity = await _context.SysUserGroups.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "模块ID不存在!" };
            }

            //将该模块下的所有模块 也进行删除
            List<SysUserGroup> deletes = new List<SysUserGroup>();
            await Task.Run(() =>
            {
                deletes = RecursiveList(_context.SysUserGroups.ToList(), entity);
            });
            await Task.Run(() =>
            {
                foreach (var obj in deletes)
                {
                    obj.Delete_Flag = true;
                    obj.Delete_By = currentUserId;
                    obj.Delete_Time = DateTime.Now;
                    _context.SysUserGroups.Update(entity);
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
                    Console.WriteLine(ex);
                    trans.Rollback();
                    return new FuncResult() { IsSuccess = false, Message = $"删除模块[{entity.UserGroupName}]时发生预料之外的错误,请重试" };

                }
            }

            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
    
        public async Task<FuncResult> Add(UserGroupModel model, int currentUserId)
        {

            //模块名称不能重复
            if (_context.SysUserGroups.FirstOrDefault(e => e.UserGroupName == model.Name) != null)
            {
                return new FuncResult() { IsSuccess = false, Message = "模块名不能重复!" };
            }
            int level = 1;
            if (!string.IsNullOrWhiteSpace(model.ParentId))
            {
                var parent_entity = _context.SysUserGroups.FirstOrDefault(e => e.Id == model.ParentId);
                if (parent_entity == null)
                {
                    return new FuncResult() { IsSuccess = false, Message = "父级ID错误" };
                }
                level = parent_entity.Level + 1;
            }

            var entity = new SysUserGroup()
            {
                UserGroupName = model.Name,
                Level = level,
                ParentId = model.ParentId,


                Last_Updated_By = currentUserId,
                Last_Update_Date = DateTime.Now,

                Created_By = currentUserId,
                Creation_Date = DateTime.Now
            };
            await _context.SysUserGroups.AddAsync(entity);

            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction trans = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return new FuncResult() { IsSuccess = false, Content = ex.Message };
                }
            }
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "添加成功" };
        }

        /// <summary>
        /// 递归获取模块
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserGroupStructure>> AcquisitionModule()
        {
            var data = _context.SysUserGroups.ToList();
            var parents = data.Where(e => string.IsNullOrWhiteSpace(e.ParentId)).ToList();
            List<UserGroupStructure> list = new List<UserGroupStructure>();
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
        private UserGroupStructure Recursive(List<SysUserGroup> data, SysUserGroup parent)
        {

            var module = new UserGroupStructure()
            {
                Id = parent.Id,
                Level = parent.Level,
                Name = parent.UserGroupName
            };
            //移除自身
            var childs = data.Where(e => e.ParentId == parent.Id).ToList();
            if (childs.Count == 0)
            {
                return module;
            }
            var parent_entity = data.FirstOrDefault(e => e.Id == parent.Id);
            if (parent_entity != null)
            {
                data.Remove(parent_entity);
            }
            foreach (var child in childs)
            {
                module.ListChilds.Add(Recursive(data.Where(e => e.ParentId == child.Id).ToList(), child));
            }
            return module;
        }


        /// <summary>
        /// 递归获取模块
        /// 返回list  用于删除
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private List<SysUserGroup> RecursiveList(List<SysUserGroup> data, SysUserGroup parent)
        {

            List<SysUserGroup> sysUserGroups = new List<SysUserGroup>();
            sysUserGroups.Add(parent);
            var childs = data.Where(e => e.ParentId == parent.Id).ToList();
            if (childs.Count == 0)
            {
                return sysUserGroups;
            }
            var parent_entity = data.FirstOrDefault(e => e.Id == parent.Id);
            if (parent_entity != null)
            {
                data.Remove(parent_entity);
            }
            foreach (var child in childs)
            {
                sysUserGroups.AddRange(RecursiveList(data.Where(e => e.ParentId == child.Id).ToList(), child));
            }
            return sysUserGroups;
        }
        private string ChineseCharacter(int level)
        {
            string level_zh = "";
            switch (level)
            {
                case 1:
                    level_zh = "一";
                    break;
                case 2:
                    level_zh = "二";
                    break;
                case 3:
                    level_zh = "三";
                    break;
                case 4:
                    level_zh = "四";
                    break;
                case 5:
                    level_zh = "五";
                    break;
                default:
                    break;
            }
            level_zh += "级用户组";
            return level_zh;
        }
    }
}
