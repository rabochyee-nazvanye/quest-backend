using Microsoft.EntityFrameworkCore.Migrations;

namespace Quest.DAL.Migrations
{
    public partial class QuestParticipantFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Quests_SoloInfiniteQuestId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Quests_TeamScheduledQuestId",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_SoloInfiniteQuestId",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_TeamScheduledQuestId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "SoloInfiniteQuestId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "TeamScheduledQuestId",
                table: "Participants");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoloInfiniteQuestId",
                table: "Participants",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamScheduledQuestId",
                table: "Participants",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participants_SoloInfiniteQuestId",
                table: "Participants",
                column: "SoloInfiniteQuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_TeamScheduledQuestId",
                table: "Participants",
                column: "TeamScheduledQuestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Quests_SoloInfiniteQuestId",
                table: "Participants",
                column: "SoloInfiniteQuestId",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Quests_TeamScheduledQuestId",
                table: "Participants",
                column: "TeamScheduledQuestId",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
