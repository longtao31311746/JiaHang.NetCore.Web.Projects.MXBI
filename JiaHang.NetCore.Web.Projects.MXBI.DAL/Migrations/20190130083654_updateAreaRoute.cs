using Microsoft.EntityFrameworkCore.Migrations;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.Migrations
{
    public partial class updateAreaRoute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CONTROLLERROUTEID",
                table: "SYS_METHOD_ROUTE",
                newName: "CONTROLLERID");

            migrationBuilder.RenameColumn(
                name: "AREAROUTEID",
                table: "SYS_CONTROLLER_ROUTE",
                newName: "AREAID");

            migrationBuilder.AlterColumn<string>(
                name: "METHODTYPE",
                table: "SYS_METHOD_ROUTE",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CONTROLLERID",
                table: "SYS_METHOD_ROUTE",
                newName: "CONTROLLERROUTEID");

            migrationBuilder.RenameColumn(
                name: "AREAID",
                table: "SYS_CONTROLLER_ROUTE",
                newName: "AREAROUTEID");

            migrationBuilder.AlterColumn<int>(
                name: "METHODTYPE",
                table: "SYS_METHOD_ROUTE",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
