using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.Relation;

namespace JiaHang.NetCore.Web.Projects.MXBI.BLL.Relation
{
    public class SysUserGroupRelationBLL
    {
        private readonly DataContext _context;
        public SysUserGroupRelationBLL(DataContext dataContext)
        {
            _context = dataContext;
        }

        public FuncResult Select()
        {
            var query = from a in _context.SysUserGroups
                        join b in _context.SysUserGroupRelations on a.Id equals b.UserGroupId
                       into b_temp
                        from b_ifnull in b_temp.DefaultIfEmpty()
                        join c in _context.SysUserInfos on b_ifnull.UserId equals c.User_Id
                        into c_temp
                        from c_ifnull in c_temp.DefaultIfEmpty()
                        orderby a.UserGroupName
                        select new
                        {
                            SysUserGroupName = a.UserGroupName,
                            SysUserGroupId = a.Id,

                            userid = c_ifnull != null ? c_ifnull.User_Id : 0,
                            username = c_ifnull != null ? c_ifnull.User_Name : null,
                            UserAccount = c_ifnull != null ? c_ifnull.User_Account : null,
                            UserMobileNo = c_ifnull != null ? c_ifnull.User_Mobile_No : null,
                            UserIsLdap = c_ifnull != null ? c_ifnull.User_Is_Ldap : false,
                            RelationId = b_ifnull != null ? b_ifnull.Id : null,

                            UserExists = c_ifnull != null
                        };
            var data = query.GroupBy(e => e.SysUserGroupId).Select(c => new
            {
                id = c.Key,
                name = c.First().SysUserGroupName,
                Children = c.Where(w => w.UserExists).Select(s => new
                {
                    s.RelationId,
                    s.UserAccount,
                    s.UserMobileNo,
                    s.UserIsLdap,
                    id = s.userid,
                    name = s.username
                })
            });


            return new FuncResult() { IsSuccess = true, Content = data };
        }

        public FuncResult NotBindUser(string groupId)
        {
            //var data=_context.SysUserGroupRelations.Where(e=>e.UserGroupId)

            List<int> ids = _context.SysUserGroupRelations.
                Where(e => string.IsNullOrWhiteSpace(groupId) || e.UserGroupId == groupId).Select(e => e.UserId).ToList();

            var data = _context.SysUserInfos.Where(e => !ids.Contains(e.User_Id)).Select(c => new
            {
                id = c.User_Id,
                name = c.User_Name,
                UserAccount = c.User_Account,
                UserIsLdap = c.User_Is_Ldap,
                UserMobileNo = c.User_Mobile_No
            });
            return new FuncResult() { IsSuccess = true, Content = data };
        }


        public async Task<FuncResult> Add(List<SysUserGroupRelationModel> model, int current_userid)
        {

            //var data=_context.SysUserGroupRelations.Where(e=>model e.UserGroupId )

            //检查groupid 是否正确
            if (_context.SysUserGroups.Count(e => model.Select(s => s.GroupId).Contains(e.Id)) != model.Select(e => e.GroupId).Distinct().Count())
            {
                return new FuncResult() { IsSuccess = false, Message = "请传递正确SysUserGroupId" };
            }

            //检查userid 是否正确
            if (_context.SysUserInfos.Count(e => model.Select(s => s.UserId).Contains(e.User_Id)) != model.Select(e => e.UserId).Distinct().Count())
            {
                return new FuncResult() { IsSuccess = false, Message = "请传递正确UserId" };
            }
            List<SysUserGroupRelation> add_entitys = new List<SysUserGroupRelation>();
            await Task.Run(() =>
            {
                foreach (SysUserGroupRelationModel obj in model)
                {
                    if (_context.SysUserGroupRelations.FirstOrDefault(e => e.UserGroupId == obj.GroupId && e.UserId == obj.UserId) == null)
                    {
                        add_entitys.Add(new SysUserGroupRelation()
                        {
                            Id = Guid.NewGuid().ToString("N"),
                            UserGroupId = obj.GroupId,
                            UserId = obj.UserId,

                            Created_By = current_userid,
                            Creation_Date = DateTime.Now,
                            Last_Updated_By = current_userid,
                            Last_Update_Date = DateTime.Now
                        });
                    }
                }
            });

            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction trans = _context.Database.BeginTransaction())
            {

                try
                {
                    await _context.SysUserGroupRelations.AddRangeAsync(add_entitys);
                    await _context.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogService.WriteError(ex.Message);
                    return new FuncResult() { IsSuccess = false, Message = "发生了预料之外的错误" };
                }
            }

            return new FuncResult() { IsSuccess = true, Message = "添加成功" };
        }

        public async Task<FuncResult> Delete(string id, int current_userId)
        {

            SysUserGroupRelation entity = await _context.SysUserGroupRelations.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "id错误" };
            }
            entity.Delete_Flag = true;
            entity.Delete_Time = DateTime.Now;

            _context.Update(entity);
            _context.SaveChanges();
            return new FuncResult() { IsSuccess = false, Message = "删除成功" };

        }
    }
}
