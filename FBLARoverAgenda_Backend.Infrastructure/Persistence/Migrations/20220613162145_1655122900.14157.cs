using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBLARoverAgenda_Backend.Infrastructure.Persistence.Migrations
{
    public partial class _165512290014157 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Suffix",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Suffix",
                table: "Teachers");
        }
    }
}
