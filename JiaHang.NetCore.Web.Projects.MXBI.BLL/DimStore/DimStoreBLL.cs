using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity;
using JiaHang.NetCore.Web.Projects.MXBI.Model;
using JiaHang.NetCore.Web.Projects.MXBI.Model.Enumerations;
using JiaHang.NetCore.Web.Projects.MXBI.Model.DimStore.RequestModel;

namespace JiaHang.NetCore.Web.Projects.MXBI.BLL
{
    public class DimStoreBLL
    {
        private readonly DataContext _context;
        public DimStoreBLL(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(SearchDimStoreModel model)
        {
            IOrderedQueryable<DimStore> query = _context.DimStore.
                        Where(a =>
                        (
                        (string.IsNullOrWhiteSpace(model.StCode) || a.stCode.Contains(model.StCode))
                        && (string.IsNullOrWhiteSpace(model.StName) || a.stName.Contains(model.StName))
                        )
                        ).OrderByDescending(e => e.stCode);
            int total = query.Count();
            var data = query.Skip(model.limit * model.page).Take(model.limit).ToList().Select(e => new
            {
                e.id,
                e.stCode,
                e.stRegion,
                e.stCity,
                e.stLocation,
                e.stBrand,
                e.stOM,
                e.stDM,
                e.stName,
                e.stName_en,
                e.stAddress,
                e.stAddress_en,
                e.stType,
                stOpenDate = e.stOpenDate != null ? Convert.ToDateTime(e.stOpenDate).ToShortDateString() : "",
                stCloseDate = e.stCloseDate != null ? Convert.ToDateTime(e.stCloseDate).ToShortDateString() : "",
                e.stTel,
                e.stFax,
                e.stEmail,
                e.stIP,
                e.stSpace,
                e.stSeat,
                e.stBizTime,
                e.stPOST,
                e.stSM,
                e.stSMTel,
                e.stComments
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
        public async Task<FuncResult> Update(int id, DimStoreModel model, int currentuserId)
        {
            DimStore entity = await _context.DimStore.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "店铺ID错误!" };
            }
            
            entity.stCode = model.stCode;
            entity.stRegion = model.stRegion;
            entity.stCity = model.stCity;
            entity.stLocation = model.stLocation;
            entity.stBrand = model.stBrand;
            entity.stOM = model.stOM;
            entity.stDM = model.stDM;
            entity.stName = model.stName;
            entity.stName_en = model.stName_en;
            entity.stAddress = model.stAddress;
            entity.stAddress_en = model.stAddress_en;
            entity.stType = model.stType;
            entity.stOpenDate = model.stOpenDate;
            entity.stCloseDate = model.stCloseDate;
            entity.stTel = model.stTel;
            entity.stFax = model.stFax;
            entity.stEmail = model.stEmail;
            entity.stIP = model.stIP;
            entity.stSpace = model.stSpace;
            entity.stSeat = model.stSeat;
            entity.stBizTime = model.stBizTime;
            entity.stPOST = model.stPOST;
            entity.stSM = model.stSM;
            entity.stSMTel = model.stSMTel;
            entity.stComments = model.stComments;
            
            entity.Last_Updated_By = currentuserId;
            entity.Last_Update_Date = DateTime.Now;

            _context.DimStore.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        }

        public async Task<FuncResult> Delete(int id, int currentUserId)
        {
            DimStore entity = await _context.DimStore.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "店铺ID不存在!" };
            }
            entity.Delete_Flag = true;
            entity.Delete_By = currentUserId;
            entity.Delete_Time = DateTime.Now;
            _context.DimStore.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        public async Task<FuncResult> Delete(int[] ids, int currentuserId)
        {
            IQueryable<DimStore> entitys = _context.DimStore.Where(e => ids.Contains(e.id));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (DimStore obj in entitys)
            {
                obj.Delete_By = currentuserId;
                obj.Delete_Flag = true;
                obj.Delete_Time = DateTime.Now;
                _context.DimStore.Update(obj);
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
        public async Task<FuncResult> Add(DimStoreModel model, int currentUserId)
        {
            if (_context.DimStore.Count(e => e.stCode == model.stCode) > 0) {
                return new FuncResult() { IsSuccess = false, Message = "已经存在相同的店铺编码。" };
            }
            DimStore entity = new DimStore
            {
                stCode = model.stCode,
                stRegion = model.stRegion,
                stCity = model.stCity,
                stLocation = model.stLocation,
                stBrand = model.stBrand,
                stOM = model.stOM,
                stDM = model.stDM,
                stName = model.stName,
                stName_en = model.stName_en,
                stAddress = model.stAddress,
                stAddress_en = model.stAddress_en,
                stType = model.stType,
                stOpenDate = model.stOpenDate,
                stCloseDate = model.stCloseDate,
                stTel = model.stTel,
                stFax = model.stFax,
                stEmail = model.stEmail,
                stIP = model.stIP,
                stSpace = model.stSpace,
                stSeat = model.stSeat,
                stBizTime = model.stBizTime,
                stPOST = model.stPOST,
                stSM = model.stSM,
                stSMTel = model.stSMTel,
                stComments = model.stComments,
                
                Creation_Date = DateTime.Now,
                Created_By = currentUserId

            };
            await _context.DimStore.AddAsync(entity);

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

        private async Task<List<SysUserInfo>> AddTestAccount(DimStoreModel model)
        {

            List<SysUserInfo> list = new List<SysUserInfo>();
            await Task.Run(() =>
            {
                for (var i = 0; i < 100; i++)
                {
                    SysUserInfo entity = new SysUserInfo
                    {
                        //User_Account = Guid.NewGuid().ToString("N").Substring(0, 6),
                        //User_Name = Guid.NewGuid().ToString("N").Substring(0, 6),
                        //User_Password = model.UserPassword,
                        //User_Org_Id = model.UserOrgId,
                        //User_Group_Names = model.UserGroupNames,
                        //User_Email = Guid.NewGuid().ToString("N").Substring(0, 6) + "@gmail.com",
                        //User_Is_Ldap = model.UserIsLdap,
                        //User_Mobile_No = model.UserMobileNo,
                        //User_Ower = model.UserOwer,
                        //Language_Code = model.LanguageCode,
                        //User_Is_Lock = model.UserIsLock,
                        //Eff_Start_Date = model.EffStartDate,
                        //Eff_End_Date = model.EffEndDate,

                        Creation_Date = DateTime.Now,
                        Created_By = 1

                    };
                    list.Add(entity);
                }
            });
            return list;

        }
    }
}
