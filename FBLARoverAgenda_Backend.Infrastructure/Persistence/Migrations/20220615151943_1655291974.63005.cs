using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBLARoverAgenda_Backend.Infrastructure.Persistence.Migrations
{
    public partial class _165529197463005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_AspNetUsers_UserId1",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_UserId1",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Classes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Classes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_UserId",
                table: "Classes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_AspNetUsers_UserId",
                table: "Classes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_AspNetUsers_UserId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_UserId",
                table: "Classes");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Classes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Classes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_UserId1",
                table: "Classes",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_AspNetUsers_UserId1",
                table: "Classes",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
