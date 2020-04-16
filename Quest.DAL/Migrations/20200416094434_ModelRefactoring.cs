using Microsoft.EntityFrameworkCore.Migrations;

namespace Quest.DAL.Migrations
{
    public partial class ModelRefactoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Quests_QuestEntityId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "AppUserQuests");

            migrationBuilder.DropIndex(
                name: "IX_Teams_QuestEntityId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "InviteTokenSecret",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "QuestEntityId",
                table: "Teams");

            migrationBuilder.AddColumn<string>(
                name: "CaptainUserId",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestId",
                table: "Teams",
                nullable: false,
                defaultValue: 7);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CaptainUserId",
                table: "Teams",
                column: "CaptainUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_QuestId",
                table: "Teams",
                column: "QuestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_AspNetUsers_CaptainUserId",
                table: "Teams",
                column: "CaptainUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Quests_QuestId",
                table: "Teams",
                column: "QuestId",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_AspNetUsers_CaptainUserId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Quests_QuestId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_CaptainUserId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_QuestId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "CaptainUserId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "QuestId",
                table: "Teams");

            migrationBuilder.AddColumn<string>(
                name: "InviteTokenSecret",
                table: "Teams",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestEntityId",
                table: "Teams",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppUserQuests",
                columns: table => new
                {
                    QuestId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserQuests", x => new { x.QuestId, x.UserId });
                    table.ForeignKey(
                        name: "FK_AppUserQuests_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserQuests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_QuestEntityId",
                table: "Teams",
                column: "QuestEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserQuests_UserId",
                table: "AppUserQuests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Quests_QuestEntityId",
                table: "Teams",
                column: "QuestEntityId",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
