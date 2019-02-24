using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity;

namespace JiaHang.NetCore.Web.Projects.MXBI.BLL.Relation
{
    public class SysModuleRouteRelationBLL
    {
        private readonly DataContext _context;

        public SysModuleRouteRelationBLL(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<FuncResult> Select()
        {
            var query = from a in _context.SysModules
                        join b in _context.SysModuleRouteRelations
                        on a.Id equals b.ModuleId
                        into b_temp
                        from b_ifnull in b_temp.DefaultIfEmpty()

                        join c in _context.SysControllerRoutes on b_ifnull.ControllerRouteId equals c.Id
                        into c_temp
                        from c_ifnull in c_temp.DefaultIfEmpty()

                        join d in _context.SysAreaRoutes on c_ifnull.AreaId equals d.Id
                        into d_temp
                        from d_ifnull in d_temp.DefaultIfEmpty()

                        join e in _context.SysMethodRoutes on c_ifnull.Id equals e.ControllerId
                        into e_temp
                        from e_ifnull in e_temp.DefaultIfEmpty()
                        orderby a.ModuleName
                        select new
                        {
                            RelationId=b_ifnull==null?null:b_ifnull.Id,
                            ModuleId = a.Id,
                            ModuleName = a.ModuleName,
                            ModuleLevel = a.Level,
                            ModuleParentId = a.ParentId,

                            ControllerIsNull = c_ifnull == null,
                            ControllerId = c_ifnull == null ? null : c_ifnull.Id,
                            ControllerPath = c_ifnull == null ? null : c_ifnull.ControllerPath,
                            ControllerAlias = c_ifnull == null ? null : c_ifnull.ControllerAlias,
                            ControllerIsApi = c_ifnull == null ? false : c_ifnull.IsApi,

                            AreaIsNull = d_ifnull == null,
                            AreaAlias = d_ifnull == null ? null : d_ifnull.AreaAlias,
                            AreaPath = d_ifnull == null ? null : d_ifnull.AreaPath,
                            AreaId = d_ifnull == null ? null : d_ifnull.Id,

                            MethodId = e_ifnull == null ? null : e_ifnull.Id,
                            MethodPath = e_ifnull == null ? null : e_ifnull.MethodPath,
                            MethodAlias = e_ifnull == null ? null : e_ifnull.MethodAlias,
                            MethodType = e_ifnull == null ? null : e_ifnull.MethodType,
                        };

            var data = query.ToList().GroupBy(e => e.ModuleId).Select(c => new ModuleRouteRelationResponseModel
            {
              
                id = c.Key,
                label = c.First().ModuleName,
                ModuleLevel = c.First().ModuleLevel,
                ModuleParentId = c.First().ModuleParentId,
                Areas = c.Where(area => !area.AreaIsNull && !area.ControllerIsNull).GroupBy(p => p.AreaId).Select(pc => new AreaResponseModel
                {
                    AreaId = pc.Key,
                    AreaAlias = pc.First().AreaAlias,
                    AreaPath = pc.First().AreaPath,
                    Controllers = pc.GroupBy(pcg => pcg.ControllerId).Select(pcs => new ControllerResponseModel
                    {
                        RelationId = pcs.First().RelationId,
                        ControllerId = pcs.Key,
                        ControllerPath = pcs.First().ControllerPath,
                        ControllerAlias = pcs.First().ControllerAlias,
                        ControllerIsApi = pcs.First().ControllerIsApi,
                        Methods = pcs.Select(m => new MethodResponseModel
                        {
                           
                            MethodId = m.MethodId,
                            MethodAlias = m.MethodAlias,
                            MethodPath = m.MethodPath,
                            MethodType = m.MethodType,
                            CompletePath = (pcs.First().ControllerIsApi ? "/api" : "") + $"/{pc.First().AreaPath}/{pcs.First().ControllerPath}/{m.MethodPath}"
                        }).ToList()
                    }).ToList()
                }).ToList(),
                Controllers = c.Where(area => area.AreaIsNull && !area.ControllerIsNull ).GroupBy(p => p.ControllerId).Select(pcs => new ControllerResponseModel
                {
                    RelationId = pcs.First().RelationId,
                    ControllerId = pcs.Key,
                    ControllerPath = pcs.First().ControllerPath,
                    ControllerAlias = pcs.First().ControllerAlias,
                    ControllerIsApi = pcs.First().ControllerIsApi,
                    Methods = pcs.Select(m => new MethodResponseModel
                    {
                        MethodId = m.MethodId,
                        MethodAlias = m.MethodAlias,
                        MethodPath = m.MethodPath,
                        MethodType = m.MethodType,
                        CompletePath = (pcs.First().ControllerIsApi?"/api":"")+ $"/{pcs.First().ControllerPath}/{m.MethodPath}"
                    }).ToList()
                }).ToList()
            });
            var result = await AcquisitionModule(data.ToList());
            return new FuncResult() { IsSuccess = true, Content = result };
        }

        public async Task<FuncResult> AddOrUpdate(List<ModuleRouteRelationRequestModel> data,int currentUserId) {
            if (data.Count <= 0) return new FuncResult() { IsSuccess = false, Message = "请传递正确参数" };
            //不能重复绑定


            //一个controller 只能被一个模块绑定  一个模块可以绑定多个controller
            if (_context.SysModuleRouteRelations.Count(q => data.Where(e => string.IsNullOrWhiteSpace(e.Id)).Select(c => c.ControllerId).Contains(q.ControllerRouteId)) > 0) {
                return new FuncResult() { IsSuccess = false, Message = "一个Controller只能绑定至一个模块上" };
            }
            //var list = _context.SysModuleRouteRelations.Where(e => data.Select(m => m.ModuleId).Contains(e.ModuleId)).ToList();
            //if (list.Count > 0)
            //{
            //    if (list.Where(e => data.Where(c => string.IsNullOrWhiteSpace(c.Id)).Select(q => q.ControllerId).Contains(e.ControllerRouteId)).Count() > 0)
            //    {
            //        return new FuncResult() { IsSuccess = false, Message = "同一Controller不能重复绑定到同一模块上" };
            //    }                
            //}

            //检查module 是否存在

            if (_context.SysModules.Count(q => data.Select(e => e.ModuleId).Contains(q.Id)) != data.Select(e => e.ModuleId).Distinct().Count())
            {
                return new FuncResult() { IsSuccess = false, Message = "模块id错误" };
            }
            //检查 userid/usergroupid是否存在
            if (_context.SysControllerRoutes.Count(q => data.Select(e => e.ControllerId).Contains(q.Id)) != data.Select(e => e.ControllerId).Distinct().Count(q => !string.IsNullOrWhiteSpace(q)))
            {
                return new FuncResult() { IsSuccess = false, Message = "ControllerId错误" };
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
                    var add_entity = new SysModuleRouteRelation()
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        ModuleId = add.ModuleId,
                       ControllerRouteId=add.ControllerId,

                        Creation_Date = DateTime.Now,
                        Created_By = currentUserId,
                        Last_Updated_By = currentUserId,
                        Last_Update_Date = DateTime.Now
                    };
                    _context.SysModuleRouteRelations.Add(add_entity);
                }

                //更新
                foreach (var modify in modifys)
                {
                    var modify_enity = _context.SysModuleRouteRelations.Find(modify.Id);

                    modify_enity.ModuleId = modify.ModuleId;
                    modify.ControllerId = modify.ControllerId;


                    modify_enity.Last_Updated_By = currentUserId;
                    modify_enity.Last_Update_Date = DateTime.Now;
                    _context.SysModuleRouteRelations.Update(modify_enity);
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


        public FuncResult NotBindRoute()
        {
            var controllers_raw = from a in _context.SysControllerRoutes
                              where string.IsNullOrWhiteSpace(a.AreaId) && !_context.SysModuleRouteRelations.Select(c=>c.ControllerRouteId).Contains(a.Id)
                              join b in _context.SysMethodRoutes
                              on a.Id equals b.ControllerId
                              into b_temp
                              from b_ifnull in b_temp.DefaultIfEmpty()
                              select new
                              {
                                  MethodIfNull = b_ifnull == null,
                                  MethodId = b_ifnull != null ? b_ifnull.Id : null,
                                  MethodPath = b_ifnull != null ? b_ifnull.MethodPath : null,
                                  MethodAlias = b_ifnull != null ? b_ifnull.MethodAlias : null,
                                  MethodType = b_ifnull != null ? b_ifnull.MethodType : null,

                                  a.IsApi,
                                  a.ControllerPath,
                                  a.ControllerAlias,
                                  ControllerId = a.Id,
                              };
            var areas_raw = from a in _context.SysAreaRoutes
                        join b in _context.SysControllerRoutes on a.Id equals b.AreaId
                        into b_temp
                        from b_ifnull in b_temp.DefaultIfEmpty()
                        where !_context.SysModuleRouteRelations.Select(c => c.ControllerRouteId).Contains(b_ifnull.Id)

                        join c in _context.SysMethodRoutes on b_ifnull.Id equals c.ControllerId
                        into c_temp
                        from c_ifnull in c_temp.DefaultIfEmpty()
                        select new
                        {
                            AreaId = a.Id,
                            a.AreaPath,
                            a.AreaAlias,

                            ControllerIfNull = b_ifnull == null,
                            ControllerId = b_ifnull != null ? b_ifnull.Id : null,
                            IsApi = b_ifnull != null ? b_ifnull.IsApi : false,
                            ControllerPath = b_ifnull != null ? b_ifnull.ControllerPath : null,
                            ControllerAlias = b_ifnull != null ? b_ifnull.ControllerAlias : null,

                            MethodIfNull = c_ifnull == null,
                            MethodId = c_ifnull != null ? c_ifnull.Id : null,
                            MethodPath = c_ifnull != null ? c_ifnull.MethodPath : null,
                            MethodAlias = c_ifnull != null ? c_ifnull.MethodAlias : null,
                            MethodType = c_ifnull != null ? c_ifnull.MethodType : null,

                        };
            var areas = areas_raw.GroupBy(e => e.AreaId).Select(r => new
            {
                AreaId = r.Key,
                r.First().AreaPath,
                r.First().AreaAlias,
                Controllers = r.Where(cr => !cr.ControllerIfNull ).
                              GroupBy(cg => cg.ControllerId).Select(controller => new
                              {
                                  controller.First().ControllerId,
                                  controller.First().ControllerAlias,
                                  controller.First().ControllerPath,
                                  controller.First().IsApi,
                                  Methods = controller.Where(n => !n.MethodIfNull).Select(method => new
                                  {
                                      method.MethodId,
                                      method.MethodPath,
                                      method.MethodType,
                                      method.MethodAlias,
                                      CompletePath = (controller.First().IsApi ? "/api" : "") + $"/{r.First().AreaPath}/{controller.First().ControllerPath}/{method.MethodPath}"
                                  })
                              })
            });
            var controllers = controllers_raw.GroupBy(cg => cg.ControllerId).Select(controller => new
            {
                controller.First().ControllerId,
                controller.First().ControllerAlias,
                controller.First().ControllerPath,
                controller.First().IsApi,
                Methods = controller.Where(n => !n.MethodIfNull).Select(method => new
                {
                    method.MethodId,
                    method.MethodPath,
                    method.MethodType,
                    method.MethodAlias,
                    CompletePath =( controller.First().IsApi ? "/api" : "") + $"/{controller.First().ControllerPath}/{method.MethodPath}"
                })
            });
            return new FuncResult() { IsSuccess=true,Content=new { areas,controllers} };
        }



        public async Task<FuncResult> Delete(string id, int currentUserId) {
            var entity = await _context.SysModuleRouteRelations.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "删除模块与Route绑定记录时，未能根据关联id找到对应的关联记录" };
            }
            entity.Delete_Flag = true;
            entity.Delete_Time = DateTime.Now;
            entity.Delete_By = currentUserId;

            _context.SysModuleRouteRelations.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };

        }

        #region 递归获取模块
        /// <summary>
        /// 递归获取模块-router
        /// </summary>
        /// <returns></returns>
        private async Task<List<ModuleRouteRelationResponseModel>> AcquisitionModule(List<ModuleRouteRelationResponseModel> data)
        {
            var parents = data.Where(e => string.IsNullOrWhiteSpace(e.ModuleParentId)).ToList();
            List<ModuleRouteRelationResponseModel> list = new List<ModuleRouteRelationResponseModel>();
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
        private ModuleRouteRelationResponseModel Recursive(List<ModuleRouteRelationResponseModel> data, ModuleRouteRelationResponseModel parent)
        {

            var module = new ModuleRouteRelationResponseModel()
            {
                id = parent.id,
                ModuleLevel = parent.ModuleLevel,
                label = parent.label,
                Areas = parent.Areas,
                Controllers = parent.Controllers
            };
            //移除自身
            var childs = data.Where(e => e.ModuleParentId == parent.id).ToList();
            if (childs.Count == 0)
            {
                return module;
            }
            var parent_entity = data.FirstOrDefault(e => e.id == parent.id);
            if (parent_entity != null)
            {
                data.Remove(parent_entity);
            }
            foreach (var child in childs)
            {
                module.children.Add(Recursive(data.Where(e => e.ModuleParentId == child.id).ToList(), child));
            }
            return module;
        }

        #endregion

    }

}

public class ModuleRouteRelationRequestModel
{
    public string Id { get; set; }
    public string ModuleId { get; set; }
    public string ControllerId { get; set; }
}

public class ModuleRouteRelationResponseModel
{
    public string id { get; set; }
   
    public string label { get; set; }
    public int ModuleLevel { get; set; }
    public string ModuleParentId { get; set; }
    public List<AreaResponseModel> Areas { get; set; }
    public List<ControllerResponseModel> Controllers { get; set; }
    public List<ModuleRouteRelationResponseModel> children { get; set; }
    public ModuleRouteRelationResponseModel()
    {
        children = new List<ModuleRouteRelationResponseModel>();
    }
}
public class AreaResponseModel
{
    public string AreaId { get; set; }
    public string AreaAlias { get; set; }
    public string AreaPath { get; set; }
    public List<ControllerResponseModel> Controllers { get; set; }
}
public class ControllerResponseModel
{
    public string RelationId { get; set; }
    public string ControllerId { get; set; }
    public string ControllerPath { get; set; }
    public string ControllerAlias { get; set; }
    public bool ControllerIsApi { get; set; }
    public List<MethodResponseModel> Methods { get; set; }
}
public class MethodResponseModel
{
   
    public string MethodId { get; set; }
    public string MethodAlias { get; set; }
    public string MethodPath { get; set; }
    public string MethodType { get; set; }
    public string CompletePath { get; set; }
}