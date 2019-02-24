﻿// <auto-generated />
using System;
using JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.Migrations
{
    [DbContext(typeof(DataMigrationsContext))]
    [Migration("20190127081914_ewqe")]
    partial class ewqe
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity.SysAreaRoute", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<string>("AreaAlias")
                        .HasColumnName("AREAALIAS");

                    b.Property<string>("AreaPath")
                        .HasColumnName("AREAPATH");

                    b.Property<int>("Created_By")
                        .HasColumnName("CREATED_BY");

                    b.Property<DateTime>("Creation_Date")
                        .HasColumnName("CREATION_DATE");

                    b.Property<int>("Delete_By")
                        .HasColumnName("DELETE_BY");

                    b.Property<bool>("Delete_Flag")
                        .HasColumnName("DELETE_FLAG");

                    b.Property<DateTime>("Delete_Time")
                        .HasColumnName("DELETE_TIME");

                    b.Property<DateTime>("Last_Update_Date")
                        .HasColumnName("LAST_UPDATE_DATE");

                    b.Property<int>("Last_Updated_By")
                        .HasColumnName("LAST_UPDATED_BY");

                    b.HasKey("Id");

                    b.ToTable("SysAreaRoutes");
                });

            modelBuilder.Entity("JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity.SysAuthorityControl", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<int>("Created_By")
                        .HasColumnName("CREATED_BY");

                    b.Property<DateTime>("Creation_Date")
                        .HasColumnName("CREATION_DATE");

                    b.Property<int>("Delete_By")
                        .HasColumnName("DELETE_BY");

                    b.Property<bool>("Delete_Flag")
                        .HasColumnName("DELETE_FLAG");

                    b.Property<DateTime>("Delete_Time")
                        .HasColumnName("DELETE_TIME");

                    b.Property<bool>("IsAllow")
                        .HasColumnName("ISALLOW");

                    b.Property<DateTime>("Last_Update_Date")
                        .HasColumnName("LAST_UPDATE_DATE");

                    b.Property<int>("Last_Updated_By")
                        .HasColumnName("LAST_UPDATED_BY");

                    b.Property<string>("MethodId")
                        .HasColumnName("METHODID");

                    b.HasKey("Id");

                    b.ToTable("SYS_AUTHORITY_CONTROL");
                });

            modelBuilder.Entity("JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity.SysControllerRoute", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<string>("AreaRouteId")
                        .HasColumnName("AREAROUTEID");

                    b.Property<string>("ControllerAlias")
                        .HasColumnName("CONTROLLERALIAS");

                    b.Property<string>("ControllerPath")
                        .HasColumnName("CONTROLLERPATH");

                    b.Property<int>("Created_By")
                        .HasColumnName("CREATED_BY");

                    b.Property<DateTime>("Creation_Date")
                        .HasColumnName("CREATION_DATE");

                    b.Property<int>("Delete_By")
                        .HasColumnName("DELETE_BY");

                    b.Property<bool>("Delete_Flag")
                        .HasColumnName("DELETE_FLAG");

                    b.Property<DateTime>("Delete_Time")
                        .HasColumnName("DELETE_TIME");

                    b.Property<bool>("IsApi")
                        .HasColumnName("ISAPI");

                    b.Property<DateTime>("Last_Update_Date")
                        .HasColumnName("LAST_UPDATE_DATE");

                    b.Property<int>("Last_Updated_By")
                        .HasColumnName("LAST_UPDATED_BY");

                    b.HasKey("Id");

                    b.ToTable("SYS_CONTROLLER_ROUTE");
                });

            modelBuilder.Entity("JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity.SysMethodRoute", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<string>("ControllerRouteId")
                        .HasColumnName("CONTROLLERROUTEID");

                    b.Property<int>("Created_By")
                        .HasColumnName("CREATED_BY");

                    b.Property<DateTime>("Creation_Date")
                        .HasColumnName("CREATION_DATE");

                    b.Property<int>("Delete_By")
                        .HasColumnName("DELETE_BY");

                    b.Property<bool>("Delete_Flag")
                        .HasColumnName("DELETE_FLAG");

                    b.Property<DateTime>("Delete_Time")
                        .HasColumnName("DELETE_TIME");

                    b.Property<DateTime>("Last_Update_Date")
                        .HasColumnName("LAST_UPDATE_DATE");

                    b.Property<int>("Last_Updated_By")
                        .HasColumnName("LAST_UPDATED_BY");

                    b.Property<string>("MethodAlias")
                        .HasColumnName("METHODALIAS");

                    b.Property<string>("MethodPath")
                        .HasColumnName("METHODPATH");

                    b.Property<int>("MethodType")
                        .HasColumnName("METHODTYPE");

                    b.HasKey("Id");

                    b.ToTable("SYS_METHOD_ROUTE");
                });

            modelBuilder.Entity("JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity.SysModelGroup", b =>
                {
                    b.Property<int>("Model_Group_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("MODEL_GROUP_ID");

                    b.Property<string>("Biz_Sys_Code")
                        .HasColumnName("BIZ_SYS_CODE")
                        .HasMaxLength(30);

                    b.Property<int>("Created_By")
                        .HasColumnName("CREATED_BY");

                    b.Property<DateTime>("Creation_Date")
                        .HasColumnName("CREATION_DATE");

                    b.Property<int>("Delete_By")
                        .HasColumnName("DELETE_BY");

                    b.Property<bool>("Delete_Flag")
                        .HasColumnName("DELETE_FLAG");

                    b.Property<DateTime>("Delete_Time")
                        .HasColumnName("DELETE_TIME");

                    b.Property<bool>("Enable_Flag")
                        .HasColumnName("ENABLE_FLAG");

                    b.Property<string>("Group_Belong")
                        .HasColumnName("GROUP_BELONG")
                        .HasMaxLength(30);

                    b.Property<string>("Image_Url")
                        .HasColumnName("IMAGE_URL")
                        .HasMaxLength(300);

                    b.Property<DateTime>("Last_Update_Date")
                        .HasColumnName("LAST_UPDATE_DATE");

                    b.Property<int>("Last_Updated_By")
                        .HasColumnName("LAST_UPDATED_BY");

                    b.Property<string>("Model_Group_Code")
                        .HasColumnName("MODEL_GROUP_CODE")
                        .HasMaxLength(30);

                    b.Property<string>("Model_Group_Name")
                        .HasColumnName("MODEL_GROUP_NAME")
                        .HasMaxLength(60);

                    b.Property<int>("Parent_Id")
                        .HasColumnName("PARENT_ID");

                    b.Property<int>("Sort_Flag")
                        .HasColumnName("SORT_FLAG");

                    b.HasKey("Model_Group_Id");

                    b.ToTable("SYS_MODEL_GROUP");
                });

            modelBuilder.Entity("JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity.SysModule", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<int>("Created_By")
                        .HasColumnName("CREATED_BY");

                    b.Property<DateTime>("Creation_Date")
                        .HasColumnName("CREATION_DATE");

                    b.Property<int>("Delete_By")
                        .HasColumnName("DELETE_BY");

                    b.Property<bool>("Delete_Flag")
                        .HasColumnName("DELETE_FLAG");

                    b.Property<DateTime>("Delete_Time")
                        .HasColumnName("DELETE_TIME");

                    b.Property<DateTime>("Last_Update_Date")
                        .HasColumnName("LAST_UPDATE_DATE");

                    b.Property<int>("Last_Updated_By")
                        .HasColumnName("LAST_UPDATED_BY");

                    b.Property<int>("Level")
                        .HasColumnName("LEVEL");

                    b.Property<string>("ModuleName")
                        .HasColumnName("MODULENAME")
                        .HasMaxLength(30);

                    b.Property<string>("ParentId")
                        .HasColumnName("PARENTID");

                    b.HasKey("Id");

                    b.ToTable("SYS_MODULE");
                });

            modelBuilder.Entity("JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity.SysModuleRouteRelation", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<string>("ControllerRouteId")
                        .HasColumnName("CONTROLLERROUTEID");

                    b.Property<int>("Created_By")
                        .HasColumnName("CREATED_BY");

                    b.Property<DateTime>("Creation_Date")
                        .HasColumnName("CREATION_DATE");

                    b.Property<int>("Delete_By")
                        .HasColumnName("DELETE_BY");

                    b.Property<bool>("Delete_Flag")
                        .HasColumnName("DELETE_FLAG");

                    b.Property<DateTime>("Delete_Time")
                        .HasColumnName("DELETE_TIME");

                    b.Property<DateTime>("Last_Update_Date")
                        .HasColumnName("LAST_UPDATE_DATE");

                    b.Property<int>("Last_Updated_By")
                        .HasColumnName("LAST_UPDATED_BY");

                    b.Property<string>("ModuleId")
                        .HasColumnName("MODULEID");

                    b.HasKey("Id");

                    b.ToTable("SYS_MODULE_ROUTE_RELATION");
                });

            modelBuilder.Entity("JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity.SysModuleUserRelation", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<int>("Created_By")
                        .HasColumnName("CREATED_BY");

                    b.Property<DateTime>("Creation_Date")
                        .HasColumnName("CREATION_DATE");

                    b.Property<int>("Delete_By")
                        .HasColumnName("DELETE_BY");

                    b.Property<bool>("Delete_Flag")
                        .HasColumnName("DELETE_FLAG");

                    b.Property<DateTime>("Delete_Time")
                        .HasColumnName("DELETE_TIME");

                    b.Property<DateTime>("Last_Update_Date")
                        .HasColumnName("LAST_UPDATE_DATE");

                    b.Property<int>("Last_Updated_By")
                        .HasColumnName("LAST_UPDATED_BY");

                    b.Property<string>("ModuleId")
                        .HasColumnName("MODULEID");

                    b.Property<int>("ModuleUserRelation")
                        .HasColumnName("MODULEUSERRELATION");

                    b.Property<int>("PermissionType")
                        .HasColumnName("PERMISSIONTYPE");

                    b.Property<string>("UserId")
                        .HasColumnName("USERID");

                    b.HasKey("Id");

                    b.ToTable("SYS_MODULE_USER_RELATION");
                });

            modelBuilder.Entity("JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity.SysSystemInfo", b =>
                {
                    b.Property<int>("System_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("SYSTEM_ID");

                    b.Property<string>("System_Code")
                        .HasColumnName("SYSTEM_CODE")
                        .HasMaxLength(30);

                    b.Property<string>("System_Name")
                        .HasColumnName("SYSTEM_NAME")
                        .HasMaxLength(60);

                    b.Property<string>("System_Url")
                        .HasColumnName("SYSTEM_URL")
                        .HasMaxLength(60);

                    b.HasKey("System_Id");

                    b.ToTable("SYS_SYSTEM_INFO");
                });

            modelBuilder.Entity("JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity.SysUserGroup", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<int>("Created_By")
                        .HasColumnName("CREATED_BY");

                    b.Property<DateTime>("Creation_Date")
                        .HasColumnName("CREATION_DATE");

                    b.Property<int>("Delete_By")
                        .HasColumnName("DELETE_BY");

                    b.Property<bool>("Delete_Flag")
                        .HasColumnName("DELETE_FLAG");

                    b.Property<DateTime>("Delete_Time")
                        .HasColumnName("DELETE_TIME");

                    b.Property<DateTime>("Last_Update_Date")
                        .HasColumnName("LAST_UPDATE_DATE");

                    b.Property<int>("Last_Updated_By")
                        .HasColumnName("LAST_UPDATED_BY");

                    b.Property<int>("Level")
                        .HasColumnName("LEVEL");

                    b.Property<string>("ParentId")
                        .HasColumnName("PARENTID");

                    b.Property<string>("UserGroupName")
                        .HasColumnName("USERGROUPNAME");

                    b.HasKey("Id");

                    b.ToTable("SYS_USER_GROUP");
                });

            modelBuilder.Entity("JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity.SysUserGroupRelation", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<int>("Created_By")
                        .HasColumnName("CREATED_BY");

                    b.Property<DateTime>("Creation_Date")
                        .HasColumnName("CREATION_DATE");

                    b.Property<int>("Delete_By")
                        .HasColumnName("DELETE_BY");

                    b.Property<bool>("Delete_Flag")
                        .HasColumnName("DELETE_FLAG");

                    b.Property<DateTime>("Delete_Time")
                        .HasColumnName("DELETE_TIME");

                    b.Property<DateTime>("Last_Update_Date")
                        .HasColumnName("LAST_UPDATE_DATE");

                    b.Property<int>("Last_Updated_By")
                        .HasColumnName("LAST_UPDATED_BY");

                    b.Property<string>("UserGroupId")
                        .HasColumnName("USERGROUPID");

                    b.Property<string>("UserId")
                        .HasColumnName("USERID");

                    b.HasKey("Id");

                    b.ToTable("SYS_USER_GROUP_RELATION");
                });

            modelBuilder.Entity("JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity.SysUserInfo", b =>
                {
                    b.Property<int>("User_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("USER_ID");

                    b.Property<int>("Created_By")
                        .HasColumnName("CREATED_BY");

                    b.Property<DateTime>("Creation_Date")
                        .HasColumnName("CREATION_DATE");

                    b.Property<int>("Delete_By")
                        .HasColumnName("DELETE_BY");

                    b.Property<bool>("Delete_Flag")
                        .HasColumnName("DELETE_FLAG");

                    b.Property<DateTime>("Delete_Time")
                        .HasColumnName("DELETE_TIME");

                    b.Property<DateTime>("Eff_End_Date")
                        .HasColumnName("EFF_END_DATE");

                    b.Property<DateTime>("Eff_Start_Date")
                        .HasColumnName("EFF_START_DATE");

                    b.Property<string>("Language_Code")
                        .HasColumnName("LANGUAGE_CODE")
                        .HasMaxLength(30);

                    b.Property<DateTime>("Last_Update_Date")
                        .HasColumnName("LAST_UPDATE_DATE");

                    b.Property<int>("Last_Updated_By")
                        .HasColumnName("LAST_UPDATED_BY");

                    b.Property<string>("User_Account")
                        .HasColumnName("USER_ACCOUNT")
                        .HasMaxLength(30);

                    b.Property<string>("User_Email")
                        .HasColumnName("USER_EMAIL")
                        .HasMaxLength(60);

                    b.Property<int>("User_Group_Names")
                        .HasColumnName("USER_GROUP_NAMES");

                    b.Property<bool>("User_Is_Ldap")
                        .HasColumnName("USER_IS_LDAP");

                    b.Property<bool>("User_Is_Lock")
                        .HasColumnName("USER_IS_LOCK");

                    b.Property<string>("User_Mobile_No")
                        .HasColumnName("USER_MOBILE_NO")
                        .HasMaxLength(30);

                    b.Property<string>("User_Name")
                        .HasColumnName("USER_NAME")
                        .HasMaxLength(50);

                    b.Property<int>("User_Org_Id")
                        .HasColumnName("USER_ORG_ID");

                    b.Property<int>("User_Ower")
                        .HasColumnName("USER_OWER");

                    b.Property<string>("User_Password")
                        .HasColumnName("USER_PASSWORD")
                        .HasMaxLength(30);

                    b.HasKey("User_Id");

                    b.ToTable("SYS_USER_INFO");
                });
#pragma warning restore 612, 618
        }
    }
}
