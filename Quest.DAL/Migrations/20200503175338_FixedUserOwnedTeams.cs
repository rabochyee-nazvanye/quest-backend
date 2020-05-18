using Microsoft.EntityFrameworkCore.Migrations;

namespace Quest.DAL.Migrations
{
    public partial class FixedUserOwnedTeams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_AspNetUsers_ApplicationUserId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_AspNetUsers_PrincipalUserId1",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_ApplicationUserId",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_PrincipalUserId1",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Participants");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Participants",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participants_ApplicationUserId",
                table: "Participants",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_PrincipalUserId1",
                table: "Participants",
                column: "PrincipalUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_AspNetUsers_ApplicationUserId",
                table: "Participants",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_AspNetUsers_PrincipalUserId1",
                table: "Participants",
                column: "PrincipalUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
