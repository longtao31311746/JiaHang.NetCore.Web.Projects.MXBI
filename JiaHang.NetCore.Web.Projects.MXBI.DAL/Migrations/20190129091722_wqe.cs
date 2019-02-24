using Microsoft.EntityFrameworkCore.Migrations;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.Migrations
{
    public partial class wqe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "USERID",
                table: "SYS_USER_GROUP_RELATION",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MODULEUSERRELATION",
                table: "SYS_USER_GROUP_RELATION",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PARENTID",
                table: "SYS_USER_GROUP_RELATION",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MODULEUSERRELATION",
                table: "SYS_USER_GROUP_RELATION");

            migrationBuilder.DropColumn(
                name: "PARENTID",
                table: "SYS_USER_GROUP_RELATION");

            migrationBuilder.AlterColumn<string>(
                name: "USERID",
                table: "SYS_USER_GROUP_RELATION",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
