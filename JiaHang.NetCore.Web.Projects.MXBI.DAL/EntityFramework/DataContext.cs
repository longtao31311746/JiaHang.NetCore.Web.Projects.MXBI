using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework
{

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {


        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType).Property<Boolean>("Delete_Flag");
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var body = Expression.Equal(
                    Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(bool) }, parameter, Expression.Constant("Delete_Flag")),
                Expression.Constant(false));


                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(body, parameter));
            }


            base.OnModelCreating(modelBuilder);
        }



        public virtual DbSet<SysUserInfo> SysUserInfos { get; set; }
        public virtual DbSet<SysSystemInfo> SysSystemInfos { get; set; }

        public virtual DbSet<SysModelGroup> SysModelGroups { get; set; }


        public virtual DbSet<SysAuthorityControl> SysAuthorityControls { get; set; }

        public virtual DbSet<SysModule> SysModules { get; set; }


        public virtual DbSet<SysModuleRouteRelation> SysModuleRouteRelations { get; set; }


        public virtual DbSet<SysModuleUserRelation> SysModuleUserRelations { get; set; }

        public virtual DbSet<SysAreaRoute> SysAreaRoutes { get; set; }



        public virtual DbSet<SysUserGroup>  SysUserGroups { get; set; }
        public virtual DbSet<SysUserGroupRelation>   SysUserGroupRelations { get; set; }


        public virtual DbSet<SysControllerRoute> SysControllerRoutes { get; set; }
        public virtual DbSet<SysMethodRoute> SysMethodRoutes { get; set; }
 
        public virtual DbSet<FactPMIX> FactPMIXes { get; set; }
        public virtual DbSet<FactTradeInfoDay> FactTradeInfoDays { get; set; }
        public virtual DbSet<FactTradeInfoHour> FactTradeInfoHours { get; set; }
        public virtual DbSet<OdsStoreMaster> OdsStoreMasters { get; set; }
        public virtual DbSet<DimProduct> DimProducts { get; set; }

        public virtual DbSet<DimStore> DimStore { get; set; }
        public virtual DbSet<FactWaste> FactWastes { get; set; }
        public virtual DbSet<FactProductionTime> FactProductionTimes { get; set; }

        public virtual DbSet<FactServiceTime> FactServiceTimes { get; set; }

        public virtual DbSet<DishColor> DishColors { get; set; } 
    }
}
