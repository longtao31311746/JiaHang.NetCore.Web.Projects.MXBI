using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.Migrations
{
    public partial class authority_control_tabless : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SYS_AUTHORITY_CONTROL",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DELETE_FLAG = table.Column<bool>(nullable: false),
                    DELETE_TIME = table.Column<DateTime>(nullable: false),
                    DELETE_BY = table.Column<int>(nullable: false),
                    CREATION_DATE = table.Column<DateTime>(nullable: false),
                    CREATED_BY = table.Column<int>(nullable: false),
                    LAST_UPDATE_DATE = table.Column<DateTime>(nullable: false),
                    LAST_UPDATED_BY = table.Column<int>(nullable: false),
                    METHODID = table.Column<int>(nullable: false),
                    ISALLOW = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_AUTHORITY_CONTROL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_CONTROLLER_ROUTE",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DELETE_FLAG = table.Column<bool>(nullable: false),
                    DELETE_TIME = table.Column<DateTime>(nullable: false),
                    DELETE_BY = table.Column<int>(nullable: false),
                    CREATION_DATE = table.Column<DateTime>(nullable: false),
                    CREATED_BY = table.Column<int>(nullable: false),
                    LAST_UPDATE_DATE = table.Column<DateTime>(nullable: false),
                    LAST_UPDATED_BY = table.Column<int>(nullable: false),
                    AREAROUTEID = table.Column<int>(nullable: true),
                    ISAPI = table.Column<bool>(nullable: false),
                    CONTROLLERPATH = table.Column<string>(nullable: true),
                    CONTROLLERALIAS = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_CONTROLLER_ROUTE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_METHOD_ROUTE",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DELETE_FLAG = table.Column<bool>(nullable: false),
                    DELETE_TIME = table.Column<DateTime>(nullable: false),
                    DELETE_BY = table.Column<int>(nullable: false),
                    CREATION_DATE = table.Column<DateTime>(nullable: false),
                    CREATED_BY = table.Column<int>(nullable: false),
                    LAST_UPDATE_DATE = table.Column<DateTime>(nullable: false),
                    LAST_UPDATED_BY = table.Column<int>(nullable: false),
                    CONTROLLERROUTEID = table.Column<int>(nullable: false),
                    METHODPATH = table.Column<string>(nullable: true),
                    METHODALIAS = table.Column<string>(nullable: true),
                    METHODTYPE = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_METHOD_ROUTE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_MODULE",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DELETE_FLAG = table.Column<bool>(nullable: false),
                    DELETE_TIME = table.Column<DateTime>(nullable: false),
                    DELETE_BY = table.Column<int>(nullable: false),
                    CREATION_DATE = table.Column<DateTime>(nullable: false),
                    CREATED_BY = table.Column<int>(nullable: false),
                    LAST_UPDATE_DATE = table.Column<DateTime>(nullable: false),
                    LAST_UPDATED_BY = table.Column<int>(nullable: false),
                    MODULENAME = table.Column<string>(maxLength: 30, nullable: true),
                    PARENTID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_MODULE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_MODULE_ROUTE_RELATION",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DELETE_FLAG = table.Column<bool>(nullable: false),
                    DELETE_TIME = table.Column<DateTime>(nullable: false),
                    DELETE_BY = table.Column<int>(nullable: false),
                    CREATION_DATE = table.Column<DateTime>(nullable: false),
                    CREATED_BY = table.Column<int>(nullable: false),
                    LAST_UPDATE_DATE = table.Column<DateTime>(nullable: false),
                    LAST_UPDATED_BY = table.Column<int>(nullable: false),
                    MODULEID = table.Column<int>(nullable: false),
                    CONTROLLERROUTEID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_MODULE_ROUTE_RELATION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_MODULE_USER_RELATION",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DELETE_FLAG = table.Column<bool>(nullable: false),
                    DELETE_TIME = table.Column<DateTime>(nullable: false),
                    DELETE_BY = table.Column<int>(nullable: false),
                    CREATION_DATE = table.Column<DateTime>(nullable: false),
                    CREATED_BY = table.Column<int>(nullable: false),
                    LAST_UPDATE_DATE = table.Column<DateTime>(nullable: false),
                    LAST_UPDATED_BY = table.Column<int>(nullable: false),
                    USERID = table.Column<int>(nullable: false),
                    MODULEUSERRELATION = table.Column<int>(nullable: false),
                    MODULEID = table.Column<int>(nullable: false),
                    PERMISSIONTYPE = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_MODULE_USER_RELATION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_USER_GROUP",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DELETE_FLAG = table.Column<bool>(nullable: false),
                    DELETE_TIME = table.Column<DateTime>(nullable: false),
                    DELETE_BY = table.Column<int>(nullable: false),
                    CREATION_DATE = table.Column<DateTime>(nullable: false),
                    CREATED_BY = table.Column<int>(nullable: false),
                    LAST_UPDATE_DATE = table.Column<DateTime>(nullable: false),
                    LAST_UPDATED_BY = table.Column<int>(nullable: false),
                    PARENTID = table.Column<int>(nullable: false),
                    USERGROUPNAME = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_USER_GROUP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_USER_GROUP_RELATION",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DELETE_FLAG = table.Column<bool>(nullable: false),
                    DELETE_TIME = table.Column<DateTime>(nullable: false),
                    DELETE_BY = table.Column<int>(nullable: false),
                    CREATION_DATE = table.Column<DateTime>(nullable: false),
                    CREATED_BY = table.Column<int>(nullable: false),
                    LAST_UPDATE_DATE = table.Column<DateTime>(nullable: false),
                    LAST_UPDATED_BY = table.Column<int>(nullable: false),
                    USERGROUPID = table.Column<int>(nullable: false),
                    USERID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_USER_GROUP_RELATION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SysAreaRoutes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DELETE_FLAG = table.Column<bool>(nullable: false),
                    DELETE_TIME = table.Column<DateTime>(nullable: false),
                    DELETE_BY = table.Column<int>(nullable: false),
                    CREATION_DATE = table.Column<DateTime>(nullable: false),
                    CREATED_BY = table.Column<int>(nullable: false),
                    LAST_UPDATE_DATE = table.Column<DateTime>(nullable: false),
                    LAST_UPDATED_BY = table.Column<int>(nullable: false),
                    AREAPATH = table.Column<string>(nullable: true),
                    AREAALIAS = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysAreaRoutes", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SYS_AUTHORITY_CONTROL");

            migrationBuilder.DropTable(
                name: "SYS_CONTROLLER_ROUTE");

            migrationBuilder.DropTable(
                name: "SYS_METHOD_ROUTE");

            migrationBuilder.DropTable(
                name: "SYS_MODULE");

            migrationBuilder.DropTable(
                name: "SYS_MODULE_ROUTE_RELATION");

            migrationBuilder.DropTable(
                name: "SYS_MODULE_USER_RELATION");

            migrationBuilder.DropTable(
                name: "SYS_USER_GROUP");

            migrationBuilder.DropTable(
                name: "SYS_USER_GROUP_RELATION");

            migrationBuilder.DropTable(
                name: "SysAreaRoutes");
        }
    }
}
