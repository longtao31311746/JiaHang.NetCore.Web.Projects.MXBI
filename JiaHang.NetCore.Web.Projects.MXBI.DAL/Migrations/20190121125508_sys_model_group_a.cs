using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.Migrations
{
    public partial class sys_model_group_a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SYS_MODEL_GROUP",
                columns: table => new
                {
                    DELETE_FLAG = table.Column<bool>(nullable: false),
                    DELETE_TIME = table.Column<DateTime>(nullable: false),
                    DELETE_BY = table.Column<int>(nullable: false),
                    CREATION_DATE = table.Column<DateTime>(nullable: false),
                    CREATED_BY = table.Column<int>(nullable: false),
                    LAST_UPDATE_DATE = table.Column<DateTime>(nullable: false),
                    LAST_UPDATED_BY = table.Column<int>(nullable: false),
                    MODEL_GROUP_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MODEL_GROUP_CODE = table.Column<string>(maxLength: 30, nullable: true),
                    MODEL_GROUP_NAME = table.Column<string>(maxLength: 60, nullable: true),
                    PARENT_ID = table.Column<int>(nullable: false),
                    SORT_FLAG = table.Column<int>(nullable: false),
                    ENABLE_FLAG = table.Column<bool>(nullable: false),
                    IMAGE_URL = table.Column<string>(maxLength: 300, nullable: true),
                    GROUP_BELONG = table.Column<string>(maxLength: 30, nullable: true),
                    BIZ_SYS_CODE = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_MODEL_GROUP", x => x.MODEL_GROUP_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SYS_MODEL_GROUP");
        }
    }
}
