using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.Enumerations;
using JiaHang.NetCore.Web.Projects.MXBI.Model.SysModelGroup.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.NetCore.Web.Projects.MXBI.BLL.SysModelGroupBLL
{
    public class SysModelGroupBLL
    {
        private readonly DataContext _context;
        public SysModelGroupBLL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(SearchSysModelGroupModel model)
        {
            var query = _context.SysModelGroups;          
            int total = query.Count();
            // var data = query.Skip(model.limit * model.page).Take(model.limit);
            var data = query.Skip(model.limit * model.page).Take(model.limit).ToList().OrderBy(c=>c.Model_Group_Name).Select(e => new
            {

                Model_Group_Id = e.Model_Group_Id,
                Model_Group_Code = e.Model_Group_Code,
                Model_Group_Name = e.Model_Group_Name,
                Parent_Id = e.Parent_Id,
                Sort_Flag = e.Sort_Flag,
                Enable_Flag = e.Enable_Flag ? "有效" : "无效",
                Image_Url = e.Image_Url,
                Group_Belong = e.Group_Belong,
                Biz_sys_Code = e.Biz_Sys_Code,
                Creation_Date = e.Creation_Date.ToShortDateString(),
                count = new Random().NextDouble() * 100
            });

            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FuncResult> Select(int id)
        {
            var entity = await _context.SysModelGroups.FindAsync(id);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(int id, SysModelGroupModel model, int currentuserId)
        {
            var entity = await _context.SysModelGroups.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "用户ID错误!" };
            }


            entity.Model_Group_Code = model.ModelGroupCode;
            entity.Model_Group_Name = model.ModelGroupName;
            entity.Parent_Id = model.ParentId;
            entity.Sort_Flag = model.SortFlag;
            entity.Enable_Flag = model.EnableFlag;
            entity.Image_Url = model.ImageUrl;
            entity.Group_Belong = model.GroupBelong;
            entity.Biz_Sys_Code = model.BizSysCode;

            entity.Last_Updated_By = currentuserId;
            entity.Last_Update_Date = DateTime.Now;


            _context.SysModelGroups.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        }
        public async Task<FuncResult> Delete(int id, int currentUserId)
        {
            var entity = await _context.SysUserInfos.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "用户ID不存在!" };
            }
            entity.Delete_Flag = true;
            entity.Delete_By = currentUserId;
            entity.Delete_Time = DateTime.Now;
            _context.SysUserInfos.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        public async Task<FuncResult> Delete(int[] ids, int currentuserId)
        {
            var entitys = _context.SysModelGroups.Where(e => ids.Contains(e.Model_Group_Id));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (var obj in entitys)
            {
                obj.Delete_By = currentuserId;
                obj.Delete_Flag = true;
                obj.Delete_Time = DateTime.Now;
                _context.SysModelGroups.Update(obj);
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
        public async Task<FuncResult> Add(SysModelGroupModel model, int currentUserId)
        {
            var entity = new SysModelGroup()
            {
                Model_Group_Code = model.ModelGroupCode,
                Model_Group_Name = model.ModelGroupName,
                Parent_Id = model.ParentId,
                Sort_Flag = model.SortFlag,
                Enable_Flag = model.EnableFlag,
                Image_Url = model.ImageUrl,
                Group_Belong = model.GroupBelong,
                Biz_Sys_Code = model.BizSysCode,

                Last_Updated_By = currentUserId,
                Last_Update_Date = DateTime.Now,

                Created_By = currentUserId,
                Creation_Date = DateTime.Now
            };
            await _context.SysModelGroups.AddAsync(entity);

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




    }
}
