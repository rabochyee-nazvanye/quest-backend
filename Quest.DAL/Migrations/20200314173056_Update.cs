using Microsoft.EntityFrameworkCore.Migrations;

namespace Quest.DAL.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestEntityId",
                table: "Teams",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_QuestEntityId",
                table: "Teams",
                column: "QuestEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Quests_QuestEntityId",
                table: "Teams",
                column: "QuestEntityId",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Quests_QuestEntityId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_QuestEntityId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "QuestEntityId",
                table: "Teams");
        }
    }
}
