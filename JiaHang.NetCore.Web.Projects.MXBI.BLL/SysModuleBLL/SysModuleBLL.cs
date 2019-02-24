using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.SysModule;
using JiaHang.NetCore.Web.Projects.MXBI.Model.SysModule.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.NetCore.Web.Projects.MXBI.BLL.SysModuleBLL
{
    public class SysModuleBLL
    {
        private readonly DataContext _context;
        public SysModuleBLL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(SearchSysModuleModel model)
        {
            var query = from a in _context.SysModules
                        where (model.ModuleLevel == 0 || a.Level == model.ModuleLevel) && (string.IsNullOrWhiteSpace(model.ModuleName) || a.ModuleName.Contains(model.ModuleName))
                        join b in _context.SysModules on a.ParentId equals b.Id
                        into a_temp
                        from a_ifnull in a_temp.DefaultIfEmpty()
                        join c in _context.SysModules on a_ifnull.ParentId equals c.Id
                        into b_temp
                        from c_ifnul in b_temp.DefaultIfEmpty()
                        orderby a.Creation_Date descending
                        select new
                        {
                            moduleId = a.Id,
                            moduleName = a.ModuleName,
                            moduleLevel = ChineseCharacter(a.Level),
                            firstModuleId = c_ifnul != null ? c_ifnul.Id : a_ifnull == null ? "" : a_ifnull.Id,
                            firstModuleName = c_ifnul != null ? c_ifnul.ModuleName : a_ifnull == null ? "未选择" : a_ifnull.ModuleName,
                            //firstModuleName = a_ifnull != null ? a_ifnull.ModuleName : c_ifnul == null ? "未选择" : c_ifnul.ModuleName,
                            firstModuleLevel = c_ifnul != null ? ChineseCharacter(c_ifnul.Level) : a_ifnull == null ? "" : ChineseCharacter(a_ifnull.Level),
                            secondModuleId = c_ifnul == null ? "" : a_ifnull.Id,
                            secondModuleName = c_ifnul == null ? "未选择" : a_ifnull.ModuleName,
                            secondModuleLevel = c_ifnul == null ? "" : ChineseCharacter(a_ifnull.Level),
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
            var entity = await _context.SysModules.FindAsync(id);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string id, SysModuleModel model, int currentuserId)
        {
            var level = 1;
            var entity = _context.SysModules.FirstOrDefault(e => e.Id == id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "模块ID错误!" };
            }

            var parent = _context.SysModules.FirstOrDefault(e => e.Id == model.ParentId);

            if (parent != null)
            {
                level = 2;
                if (_context.SysModules.FirstOrDefault(e => e.Id == parent.ParentId) != null) {
                    level = 3;
                }
            }

            //模块名称不能重复
            if (_context.SysModules.FirstOrDefault(e => e.ModuleName == model.ModuleName && e.Id != id) != null)
            {
                return new FuncResult() { IsSuccess = false, Message = "模块名不能重复!" };
            }


            entity.ParentId = model.ParentId;
            entity.ModuleName = model.ModuleName;
            entity.Level = level;
            entity.Last_Updated_By = currentuserId;
            entity.Last_Update_Date = DateTime.Now;


            _context.SysModules.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        }
        public async Task<FuncResult> Delete(string id, int currentUserId)
        {
            var entity = await _context.SysModules.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "模块ID不存在!" };
            }

            //将该模块下的所有模块 也进行删除
            List<SysModule> deletes = new List<SysModule>();
            await Task.Run(() =>
            {
                deletes = RecursiveList(_context.SysModules.ToList(), entity);
            });
            await Task.Run(() =>
            {
                foreach (var obj in deletes)
                {
                    obj.Delete_Flag = true;
                    obj.Delete_By = currentUserId;
                    obj.Delete_Time = DateTime.Now;
                    _context.SysModules.Update(entity);
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
                    return new FuncResult() { IsSuccess = false, Message = $"删除模块[{entity.ModuleName}]时发生预料之外的错误,请重试" };

                }
            }

            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        public async Task<FuncResult> Delete(string[] ids, int currentuserId)
        {
            var entitys = _context.SysModules.Where(e => ids.Contains(e.Id));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (var obj in entitys)
            {
                obj.Delete_By = currentuserId;
                obj.Delete_Flag = true;
                obj.Delete_Time = DateTime.Now;
                _context.SysModules.Update(obj);
            }
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
                    LogService.WriteError(ex);
                    return new FuncResult() { IsSuccess = false, Message = "删除时发生了意料之外的错误" };
                }
            }
            return new FuncResult() { IsSuccess = true, Message = $"已成功删除{ids.Length}条记录" };

        }
        public async Task<FuncResult> Add(SysModuleModel model, int currentUserId)
        {

            //模块名称不能重复
            if (_context.SysModules.FirstOrDefault(e => e.ModuleName == model.ModuleName) != null)
            {
                return new FuncResult() { IsSuccess = false, Message = "模块名不能重复!" };
            }
            int level = 1;
            if (!string.IsNullOrWhiteSpace(model.ParentId))
            {
                var parent_entity = _context.SysModules.FirstOrDefault(e => e.Id == model.ParentId);
                if (parent_entity == null)
                {
                    return new FuncResult() { IsSuccess = false, Message = "父级ID错误" };
                }
                level = parent_entity.Level + 1;
            }

            var entity = new SysModule()
            {
                ModuleName = model.ModuleName,
                Level = level,
                ParentId = model.ParentId,


                Last_Updated_By = currentUserId,
                Last_Update_Date = DateTime.Now,

                Created_By = currentUserId,
                Creation_Date = DateTime.Now
            };
            await _context.SysModules.AddAsync(entity);

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
        public async Task<List<ModuleStructure>> AcquisitionModule()
        {
            var data = _context.SysModules.ToList();
            var parents = data.Where(e => string.IsNullOrWhiteSpace(e.ParentId)).ToList();
            List<ModuleStructure> list = new List<ModuleStructure>();
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
        private ModuleStructure Recursive(List<SysModule> data, SysModule parent)
        {

            var module = new ModuleStructure()
            {
                Id = parent.Id,
                ModuleLevel = parent.Level,
                ModuleName = parent.ModuleName
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
        private List<SysModule> RecursiveList(List<SysModule> data, SysModule parent)
        {

            List<SysModule> sysModules = new List<SysModule>();
            sysModules.Add(parent);
            var childs = data.Where(e => e.ParentId == parent.Id).ToList();
            if (childs.Count == 0)
            {
                return sysModules;
            }
            var parent_entity = data.FirstOrDefault(e => e.Id == parent.Id);
            if (parent_entity != null)
            {
                data.Remove(parent_entity);
            }
            foreach (var child in childs)
            {
                sysModules.AddRange(RecursiveList(data.Where(e => e.ParentId == child.Id).ToList(), child));
            }
            return sysModules;
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
            level_zh += "级模块";
            return level_zh;
        }
    }
}
