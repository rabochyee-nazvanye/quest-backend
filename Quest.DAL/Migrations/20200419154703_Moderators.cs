using Microsoft.EntityFrameworkCore.Migrations;

namespace Quest.DAL.Migrations
{
    public partial class Moderators : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModeratorId",
                table: "Teams",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_ModeratorId",
                table: "Teams",
                column: "ModeratorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_AspNetUsers_ModeratorId",
                table: "Teams",
                column: "ModeratorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_AspNetUsers_ModeratorId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_ModeratorId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "ModeratorId",
                table: "Teams");
        }
    }
}
