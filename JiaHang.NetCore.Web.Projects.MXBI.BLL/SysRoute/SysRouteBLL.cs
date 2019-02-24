using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.SysRoute;

namespace JiaHang.NetCore.Web.Projects.MXBI.BLL
{
    public class SysRouteBLL
    {
        private readonly DataContext _context;
        public SysRouteBLL(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<FuncResult> Select()
        {

            var controllers = from a in _context.SysControllerRoutes
                              where string.IsNullOrWhiteSpace(a.AreaId)
                              join b in _context.SysMethodRoutes
                              on a.Id equals b.ControllerId
                              into b_temp
                              from b_ifnull in b_temp.DefaultIfEmpty()
                              orderby a.ControllerAlias
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
            var areas = from a in _context.SysAreaRoutes
                        join b in _context.SysControllerRoutes on a.Id equals b.AreaId
                        into b_temp
                        from b_ifnull in b_temp.DefaultIfEmpty()
                        join c in _context.SysMethodRoutes on b_ifnull.Id equals c.ControllerId
                        into c_temp
                        from c_ifnull in c_temp.DefaultIfEmpty()
                        orderby a.AreaAlias
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

            object data = null;
            await Task.Run(() =>
            {
                data = new
                {
                    ApiRoute = new
                    {
                        Areas = areas.GroupBy(e => e.AreaId).Select(r => new
                        {
                            AreaId = r.Key,
                            r.First().AreaPath,
                            r.First().AreaAlias,
                            Controllers = r.Where(cr => !cr.ControllerIfNull && cr.IsApi).
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
                                    CompletePath = (controller.First().IsApi?"/api":"")+ $"/{r.First().AreaPath}/{controller.First().ControllerPath}/{method.MethodPath}"
                                })
                            })
                        }),
                        Controllers = controllers.Where(e => e.IsApi).GroupBy(cg => cg.ControllerId).Select(controller => new
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
                                CompletePath = (controller.First().IsApi ? "/api" : "") + $"/{controller.First().ControllerPath}/{method.MethodPath}"
                            })
                        })
                    },
                    ViewRoute = new
                    {
                        Areas = areas.GroupBy(e => e.AreaId).Select(r => new
                        {
                            AreaId = r.Key,
                            r.First().AreaPath,
                            r.First().AreaAlias,
                            Controllers = r.Where(cr => !cr.ControllerIfNull && !cr.IsApi).GroupBy(cg => cg.ControllerId).Select(controller => new
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
                        }),
                        Controllers = controllers.Where(e => !e.IsApi).GroupBy(cg => cg.ControllerId).Select(controller => new
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
                                CompletePath = (controller.First().IsApi ? "/api" : "" )+ $"/{controller.First().ControllerPath}/{method.MethodPath}"
                            })
                        })
                    },
                };
            });

            return new FuncResult() { IsSuccess = true, Content = data };
        }


        public async Task<FuncResult> AddOrUpdateArea(AreaRouteModel model, int currentUserId)
        {
            if (string.IsNullOrWhiteSpace(model.AreaPath))
            {
                return new FuncResult() { IsSuccess = false, Message = "Area路径不能为空" };
            }

            if (!Regex.Match(model.AreaPath, @"^[a-zA-Z0-9]+$").Success)
            {
                return new FuncResult() { IsSuccess = false, Message = "AreaPath只能输入字母、数字" };
            }

            if (_context.SysAreaRoutes.Where(e => e.Id != model.Id).Select(e => e.AreaPath.ToLower()).Contains(model.AreaPath))
            {
                return new FuncResult() { IsSuccess = false, Message = "路径重复" };
            }
            if (_context.SysAreaRoutes.Where(e => e.Id != model.Id).Select(e => e.AreaAlias.ToLower()).Contains(model.AreaAlias))
            {
                return new FuncResult() { IsSuccess = false, Message = "别名重复" };
            }

            SysAreaRoute entity = new SysAreaRoute();
            if (!string.IsNullOrWhiteSpace(model.Id))
            {
                entity = await _context.SysAreaRoutes.FindAsync(model.Id);
                if (entity == null)
                {
                    return new FuncResult() { IsSuccess = false, Message = "id错误" };
                }
                entity.AreaAlias = model.AreaAlias;
                entity.AreaPath = model.AreaPath;
                entity.Last_Updated_By = currentUserId;
                entity.Last_Update_Date = DateTime.Now;
                _context.SysAreaRoutes.Update(entity);
                await _context.SaveChangesAsync();
                return new FuncResult() { IsSuccess = true, Content = entity, Message = "编辑成功" };
            }

            entity.Id = Guid.NewGuid().ToString("N");
            entity.AreaAlias = model.AreaAlias;
            entity.AreaPath = model.AreaPath;

            entity.Last_Updated_By = currentUserId;
            entity.Last_Update_Date = DateTime.Now;
            entity.Created_By = currentUserId;
            entity.Creation_Date = DateTime.Now;

            await _context.SysAreaRoutes.AddAsync(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "添加成功" };
        }


        public async Task<FuncResult> AddOrUpdateController(ControllerRouteModel model, int currentUserId)
        {

            
          
            if (string.IsNullOrWhiteSpace(model.ControllerPath))
            {
                return new FuncResult() { IsSuccess = false, Message = "Controller路径不能为空" };
            }
            if (!Regex.Match(model.ControllerPath, @"^[a-zA-Z0-9]+$").Success)
            {
                return new FuncResult() { IsSuccess = false, Message = "ControllerPath只能输入字母、数字" };
            }
            if (string.IsNullOrWhiteSpace(model.ControllerAlias))
            {
                model.ControllerAlias = model.ControllerPath;
            }
            if (_context.SysControllerRoutes.Where(e => e.Id != model.Id).Select(e => e.ControllerPath.ToLower()).Contains(model.ControllerPath))
            {
                return new FuncResult() { IsSuccess = false, Message = "路径重复" };
            }
            if (_context.SysControllerRoutes.Where(e => e.Id != model.Id).Select(e => e.ControllerAlias.ToLower()).Contains(model.ControllerAlias))
            {
                return new FuncResult() { IsSuccess = false, Message = "别名重复" };
            }
          


            SysControllerRoute entity = new SysControllerRoute();
            if (!string.IsNullOrWhiteSpace(model.Id))
            {
                entity = await _context.SysControllerRoutes.FindAsync(model.Id);
                if (entity == null)
                {
                    return new FuncResult() { IsSuccess = false, Message = "id错误" };
                }
                entity.ControllerAlias = model.ControllerAlias;
                entity.ControllerPath = model.ControllerPath;
                entity.AreaId = model.AreaId;
                entity.IsApi = model.IsApi;


                entity.Last_Updated_By = currentUserId;
                entity.Last_Update_Date = DateTime.Now;
                _context.SysControllerRoutes.Update(entity);
                await _context.SaveChangesAsync();
                return new FuncResult() { IsSuccess = true, Content = entity, Message = "编辑成功" };
            }

            entity.Id = Guid.NewGuid().ToString("N");
            entity.ControllerAlias = model.ControllerAlias;
            entity.ControllerPath = model.ControllerPath;
            entity.AreaId = model.AreaId;
            entity.IsApi = model.IsApi;

            entity.Last_Updated_By = currentUserId;
            entity.Last_Update_Date = DateTime.Now;
            entity.Created_By = currentUserId;
            entity.Creation_Date = DateTime.Now;

            await _context.SysControllerRoutes.AddAsync(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "添加成功" };
        }


        public async Task<FuncResult> AddOrUpdateMethod(MethodRouteModel model, int currentUserId)
        {
            if (await _context.SysControllerRoutes.FindAsync(model.ControllerId) == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "ControllerId错误!" };
            }
            if (string.IsNullOrWhiteSpace(model.MethodType))
            {
                return new FuncResult() { IsSuccess = false, Message = "Method路径不能为空" };
            }

            if (!Regex.Match(model.MethodPath, @"^[a-zA-Z0-9]+$").Success)
            {
                return new FuncResult() { IsSuccess = false, Message = "MethodPath只能输入字母、数字" };
            }

            if (string.IsNullOrWhiteSpace(model.MethodAlias))
            {
                model.MethodAlias = model.MethodAlias;
            }
            if (string.IsNullOrWhiteSpace(model.MethodType))
            {
                model.MethodType = "HttpGet";
            }
            if (_context.SysMethodRoutes.Where(e =>e.ControllerId==model.ControllerId&& e.Id != model.Id).Select(e => e.MethodPath.ToLower()).Contains(model.MethodPath))
            {
                return new FuncResult() { IsSuccess = false, Message = "路径重复" };
            }
            if (_context.SysMethodRoutes.Where(e => e.ControllerId == model.ControllerId && e.Id != model.Id).Select(e => e.MethodAlias.ToLower()).Contains(model.MethodAlias))
            {
                return new FuncResult() { IsSuccess = false, Message = "别名重复" };
            }



            SysMethodRoute entity = new SysMethodRoute();
            if (!string.IsNullOrWhiteSpace(model.Id))
            {
                entity = await _context.SysMethodRoutes.FindAsync(model.Id);
                if (entity == null)
                {
                    return new FuncResult() { IsSuccess = false, Message = "id错误" };
                }
                entity.MethodAlias = model.MethodAlias;
                entity.MethodPath = model.MethodPath;
                entity.ControllerId = model.ControllerId;
                entity.MethodType = model.MethodType;


                entity.Last_Updated_By = currentUserId;
                entity.Last_Update_Date = DateTime.Now;
                _context.SysMethodRoutes.Update(entity);
                await _context.SaveChangesAsync();
                return new FuncResult() { IsSuccess = true, Content = entity, Message = "编辑成功" };
            }

            entity.Id = Guid.NewGuid().ToString("N");
            entity.MethodAlias = model.MethodAlias;
            entity.MethodPath = model.MethodPath;
            entity.ControllerId = model.ControllerId;
            entity.MethodType = model.MethodType;

            entity.Last_Updated_By = currentUserId;
            entity.Last_Update_Date = DateTime.Now;
            entity.Created_By = currentUserId;
            entity.Creation_Date = DateTime.Now;

            await _context.SysMethodRoutes.AddAsync(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "添加成功" };
        }



        public async Task<FuncResult> DeleteAreaRoute(string id, int currentUserId)
        {
            SysAreaRoute entity = await _context.SysAreaRoutes.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "AreaId错误" };
            }
            if (_context.SysControllerRoutes.Count(e => e.AreaId == id) > 0)
            {
                return new FuncResult() { IsSuccess = false, Message = "该Area下还存在Controller,请先删除其所有的Controller" };
            }
            entity.Delete_Flag = true;
            entity.Delete_By = currentUserId;
            entity.Delete_Time = DateTime.Now;
            _context.SysAreaRoutes.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }


        public async Task<FuncResult> DeleteControllerRoute(string id, int currentUserId)
        {
            SysControllerRoute entity = await _context.SysControllerRoutes.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "ControllerId错误" };
            }
            if (_context.SysMethodRoutes.Count(e => e.ControllerId == id) > 0)
            {
                return new FuncResult() { IsSuccess = false, Message = "该Controller下还存在Method,请先删除其所有的Method" };
            }
            entity.Delete_Flag = true;
            entity.Delete_By = currentUserId;
            entity.Delete_Time = DateTime.Now;
            _context.SysControllerRoutes.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }

        public async Task<FuncResult> DeleteMethodRoute(string id, int currentUserId)
        {
            SysMethodRoute entity = await _context.SysMethodRoutes.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "MethodId错误" };
            }

            entity.Delete_Flag = true;
            entity.Delete_By = currentUserId;
            entity.Delete_Time = DateTime.Now;
            _context.SysMethodRoutes.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
    }
}
