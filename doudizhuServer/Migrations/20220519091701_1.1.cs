using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace doudizhuServer.Migrations
{
    public partial class _11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Users",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "ID");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
