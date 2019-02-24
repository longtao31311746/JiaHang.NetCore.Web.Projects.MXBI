using Microsoft.EntityFrameworkCore.Migrations;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.Migrations
{
    public partial class sad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "USERID",
                table: "SYS_MODULE_USER_RELATION",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "USERGROUPID",
                table: "SYS_MODULE_USER_RELATION",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "USERGROUPID",
                table: "SYS_MODULE_USER_RELATION");

            migrationBuilder.AlterColumn<string>(
                name: "USERID",
                table: "SYS_MODULE_USER_RELATION",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
