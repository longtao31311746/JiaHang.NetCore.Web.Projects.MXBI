using Microsoft.EntityFrameworkCore.Migrations;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.Migrations
{
    public partial class ewqe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LEVEL",
                table: "SYS_USER_GROUP",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LEVEL",
                table: "SYS_USER_GROUP");
        }
    }
}
