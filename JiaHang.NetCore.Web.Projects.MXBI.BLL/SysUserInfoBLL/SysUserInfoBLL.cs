using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.Enumerations;
using JiaHang.NetCore.Web.Projects.MXBI.Model.Enumerations.Sys_User;
using JiaHang.NetCore.Web.Projects.MXBI.Model.SysUserInfo.RequestModel;
using OfficeOpenXml;

namespace JiaHang.NetCore.Web.Projects.MXBI.BLL.SysUserInfoService
{
    public class SysUserInfoBLL
    {
        private readonly DataContext _context;
        public SysUserInfoBLL(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(SearchSysUserInfo model)
        {
            IOrderedQueryable<SysUserInfo> query = _context.SysUserInfos.
                        Where(a =>
                        (
                        (string.IsNullOrWhiteSpace(model.User_Account) || a.User_Account.Contains(model.User_Account))
                        && (string.IsNullOrWhiteSpace(model.User_Name) || a.User_Name.Contains(model.User_Name))
                        && (model.User_Is_Ldap == null || a.User_Is_Ldap == model.User_Is_Ldap)
                        && (model.User_Ower == null || model.User_Ower == 0 || a.User_Ower == (UserOwerType)model.User_Ower)
                        )
                        ).OrderByDescending(e => e.Creation_Date);
            int total = query.Count();
            var data = query.Skip(model.limit * model.page).Take(model.limit).ToList().Select(e => new
            {
                e.User_Id,
                e.User_Account,
                e.User_Name,
                e.User_Org_Id,
                e.User_Group_Names,
                e.User_Email,
                e.User_Is_Ldap,
                e.User_Password,
                e.User_Is_Lock,
                e.User_Mobile_No,
                User_Ower = e.User_Ower.GetDescription(),
                e.Language_Code,

                Eff_Start_Date = e.Eff_Start_Date.ToShortDateString(),
                Eff_End_Date = e.Eff_End_Date.ToShortDateString(),
                Creation_Date = e.Creation_Date.ToShortDateString()
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
            SysUserInfo entity = await _context.SysUserInfos.FindAsync(id);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(int id, SysUserInfoModel model, int currentuserId)
        {
            SysUserInfo entity = await _context.SysUserInfos.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "用户ID错误!" };
            }


            entity.User_Name = model.UserName;
            entity.User_Password = model.UserPassword;
            entity.User_Org_Id = model.UserOrgId;
            entity.User_Group_Names = model.UserGroupNames;
            entity.User_Email = model.UserEmail;
            entity.User_Is_Ldap = model.UserIsLdap;
            entity.User_Mobile_No = model.UserMobileNo;
            entity.User_Ower = model.UserOwer;
            entity.Language_Code = model.LanguageCode;
            entity.User_Is_Lock = model.UserIsLock;
            entity.Eff_Start_Date = model.EffStartDate;
            entity.Eff_End_Date = model.EffEndDate;

            entity.Last_Updated_By = currentuserId;
            entity.Last_Update_Date = DateTime.Now;

            _context.SysUserInfos.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        }

        public async Task<FuncResult> Delete(int id, int currentUserId)
        {
            SysUserInfo entity = await _context.SysUserInfos.FindAsync(id);
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
            IQueryable<SysUserInfo> entitys = _context.SysUserInfos.Where(e => ids.Contains(e.User_Id));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (SysUserInfo obj in entitys)
            {
                obj.Delete_By = currentuserId;
                obj.Delete_Flag = true;
                obj.Delete_Time = DateTime.Now;
                _context.SysUserInfos.Update(obj);
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
        public async Task<FuncResult> Add(SysUserInfoModel model,int currentUserId)
        {
            SysUserInfo entity = new SysUserInfo
            {                
                User_Account = model.UserAccount,
                User_Name = model.UserName,
                User_Password = model.UserPassword,
                User_Org_Id = model.UserOrgId,
                User_Group_Names = model.UserGroupNames,
                User_Email = model.UserEmail,
                User_Is_Ldap = model.UserIsLdap,
                User_Mobile_No = model.UserMobileNo,
                User_Ower = model.UserOwer,
                Language_Code = model.LanguageCode,
                User_Is_Lock = model.UserIsLock,
                Eff_Start_Date = model.EffStartDate,
                Eff_End_Date = model.EffEndDate,

                Creation_Date = DateTime.Now,
                Created_By = currentUserId

            };
            await _context.SysUserInfos.AddAsync(entity);

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

        public FuncResult<SysUserInfo> Login(string userAccount, string password)
        {
            var result = new FuncResult<SysUserInfo>() { IsSuccess = false, Message = "账号或密码错误!" };
            var entity = _context.SysUserInfos.FirstOrDefault(e => e.User_Account == userAccount);
            if (entity != null && entity.User_Password == password)
            {
                result.Content = entity;
                result.Message = "登录成功";
                result.IsSuccess = true;
            }
            return result;
        }
        private async Task<List<SysUserInfo>> AddTestAccount(SysUserInfoModel model)
        {   
            
            List<SysUserInfo> list = new List<SysUserInfo>();
            await Task.Run(() =>
            {
                for (var i = 0; i < 100; i++)
                {
                    SysUserInfo entity = new SysUserInfo
                    {
                        User_Account = Guid.NewGuid().ToString("N").Substring(0, 6),
                        User_Name = Guid.NewGuid().ToString("N").Substring(0, 6),
                        User_Password = model.UserPassword,
                        User_Org_Id = model.UserOrgId,
                        User_Group_Names = model.UserGroupNames,
                        User_Email = Guid.NewGuid().ToString("N").Substring(0, 6) + "@gmail.com",
                        User_Is_Ldap = model.UserIsLdap,
                        User_Mobile_No = model.UserMobileNo,
                        User_Ower = model.UserOwer,
                        Language_Code = model.LanguageCode,
                        User_Is_Lock = model.UserIsLock,
                        Eff_Start_Date = model.EffStartDate,
                        Eff_End_Date = model.EffEndDate,

                        Creation_Date = DateTime.Now,
                        Created_By = 1

                    };
                    list.Add(entity);
                }
            });
            return list;

        }


        public async Task<byte[]> GetUserListBytes()

        {
            
            var comlumHeadrs = new[] { "用户ID", "登录帐号", "用户名称", "用户组织ID", "用户组别名称", "用户电子邮件地址", "用户手机号码", "创建时间" };
            byte[] result;
            var data = await _context.ExecSpAsync<List<SysUserInfo>>("usp_GetList"); ;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1"); //Worksheet name
                //First add the headers
                for (var i = 0; i < comlumHeadrs.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                }
                //Add values
                var j = 2;
                // var chars = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                foreach (var obj in data)
                {
                    var rt = obj.GetType();
                    var rp = rt.GetProperties();
                  
                    worksheet.Cells["A" + j].Value = obj.User_Id;
                    worksheet.Cells["B" + j].Value = obj.User_Account;
                    worksheet.Cells["C" + j].Value = obj.User_Name;
                    worksheet.Cells["D" + j].Value = obj.User_Org_Id;
                    worksheet.Cells["E" + j].Value = obj.User_Group_Names;
                    worksheet.Cells["F" + j].Value = obj.User_Email;
                    worksheet.Cells["G" + j].Value = obj.User_Mobile_No;
                    worksheet.Cells["H" + j].Value = obj.Creation_Date;
                    j++;
                }
                result = package.GetAsByteArray();


            }
            return result;
        }

    }
}
