using Microsoft.EntityFrameworkCore.Migrations;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.Migrations
{
    public partial class wqwqee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MODULEUSERRELATION",
                table: "SYS_USER_GROUP_RELATION");

            migrationBuilder.DropColumn(
                name: "PARENTID",
                table: "SYS_USER_GROUP_RELATION");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
