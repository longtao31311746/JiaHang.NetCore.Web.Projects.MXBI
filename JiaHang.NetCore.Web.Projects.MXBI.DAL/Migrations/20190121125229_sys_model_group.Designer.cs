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
    [Migration("20190121125229_sys_model_group")]
    partial class sys_model_group
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity.Sys_System_Info", b =>
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

            modelBuilder.Entity("JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework.Entity.Sys_User_Info", b =>
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
