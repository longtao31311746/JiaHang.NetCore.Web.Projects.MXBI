using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework
{
    public class DataMigrationsContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //oracle 连接字符串
            //optionsBuilder.UseOracle(@"User Id=Scott;Password=tiger;Data Source=Ora;");
            //配置mariadb连接字符串
            optionsBuilder.UseMySql("Data Source=119.29.37.82;Database=MXBI;User ID=root;Password=D14C24D597174752B~;pooling=true;CharSet=utf8;port=3306;sslmode=none");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //设置所有列名为大写

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    property.Relational().ColumnName = property.Name.ToUpper();
                }
            }

        }

        public virtual DbSet<SysUserInfo> SysUserInfos { get; set; }
        public virtual DbSet<SysSystemInfo> SysSystemInfos { get; set; }
        public virtual DbSet<SysModelGroup> SysModelGroups { get; set; }
        


        public virtual DbSet<SysAuthorityControl> SysAuthorityControls { get; set; }

        public virtual DbSet<SysModule> SysModules { get; set; }

        public virtual DbSet<SysModuleRouteRelation> SysModuleRouteRelations { get; set; }

        public virtual DbSet<SysModuleUserRelation> SysModuleUserRelations { get; set; }

        public virtual DbSet<SysAreaRoute> SysAreaRoutes { get; set; }
        public virtual DbSet<SysControllerRoute> SysControllerRoutes { get; set; }
        public virtual DbSet<SysMethodRoute> SysMethodRoutes { get; set; }


        public virtual DbSet<SysUserGroup> SysUserGroups { get; set; }
        public virtual DbSet<SysUserGroupRelation> SysUserGroupRelations { get; set; }
    }

    
}
