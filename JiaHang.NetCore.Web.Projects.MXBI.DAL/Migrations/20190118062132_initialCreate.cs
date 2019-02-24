using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.Migrations
{
    public partial class initialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SYS_SYSTEM_INFO",
                columns: table => new
                {
                    SYSTEM_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SYSTEM_CODE = table.Column<string>(maxLength: 30, nullable: true),
                    SYSTEM_NAME = table.Column<string>(maxLength: 60, nullable: true),
                    SYSTEM_URL = table.Column<string>(maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_SYSTEM_INFO", x => x.SYSTEM_ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_USER_INFO",
                columns: table => new
                {
                    DELETE_FLAG = table.Column<bool>(nullable: false),
                    DELETE_TIME = table.Column<DateTime>(nullable: false),
                    DELETE_BY = table.Column<int>(nullable: false),
                    CREATION_DATE = table.Column<DateTime>(nullable: false),
                    CREATED_BY = table.Column<int>(nullable: false),
                    LAST_UPDATE_DATE = table.Column<DateTime>(nullable: false),
                    LAST_UPDATED_BY = table.Column<int>(nullable: false),
                    USER_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    USER_ACCOUNT = table.Column<string>(maxLength: 30, nullable: true),
                    USER_NAME = table.Column<string>(maxLength: 50, nullable: true),
                    USER_PASSWORD = table.Column<string>(maxLength: 30, nullable: true),
                    USER_ORG_ID = table.Column<int>(nullable: false),
                    USER_GROUP_NAMES = table.Column<int>(nullable: false),
                    USER_EMAIL = table.Column<string>(maxLength: 60, nullable: true),
                    USER_IS_LDAP = table.Column<bool>(nullable: false),
                    USER_MOBILE_NO = table.Column<string>(maxLength: 30, nullable: true),
                    USER_OWER = table.Column<int>(nullable: false),
                    LANGUAGE_CODE = table.Column<string>(maxLength: 30, nullable: true),
                    USER_IS_LOCK = table.Column<bool>(nullable: false),
                    EFF_START_DATE = table.Column<DateTime>(nullable: false),
                    EFF_END_DATE = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_USER_INFO", x => x.USER_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SYS_SYSTEM_INFO");

            migrationBuilder.DropTable(
                name: "SYS_USER_INFO");
        }
    }
}
