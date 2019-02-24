using Microsoft.EntityFrameworkCore.Migrations;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.Migrations
{
    public partial class updateAreaRouteewq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SysAreaRoutes",
                table: "SysAreaRoutes");

            migrationBuilder.RenameTable(
                name: "SysAreaRoutes",
                newName: "SYS_AREA_ROUTE");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SYS_AREA_ROUTE",
                table: "SYS_AREA_ROUTE",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SYS_AREA_ROUTE",
                table: "SYS_AREA_ROUTE");

            migrationBuilder.RenameTable(
                name: "SYS_AREA_ROUTE",
                newName: "SysAreaRoutes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SysAreaRoutes",
                table: "SysAreaRoutes",
                column: "ID");
        }
    }
}
