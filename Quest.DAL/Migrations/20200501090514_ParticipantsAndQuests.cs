using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quest.DAL.Migrations
{
    public partial class ParticipantsAndQuests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAttempts_Teams_TeamId",
                table: "TaskAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_AspNetUsers_CaptainUserId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_AspNetUsers_ModeratorId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Quests_QuestId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamUsers_Teams_TeamId",
                table: "TeamUsers");

            migrationBuilder.DropTable(
                name: "UsedTeamHints");

            migrationBuilder.DropIndex(
                name: "IX_TaskAttempts_TeamId",
                table: "TaskAttempts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teams",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_CaptainUserId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "TaskAttempts");

            migrationBuilder.DropColumn(
                name: "CaptainUserId",
                table: "Teams");

            migrationBuilder.RenameTable(
                name: "Teams",
                newName: "Participants");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_QuestId",
                table: "Participants",
                newName: "IX_Participants_QuestId");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_ModeratorId",
                table: "Participants",
                newName: "IX_Participants_ModeratorId");

            migrationBuilder.AddColumn<int>(
                name: "ParticipantId",
                table: "TaskAttempts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Quests",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<bool>(
                name: "ResultsAvailable",
                table: "Quests",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDeadline",
                table: "Quests",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "MaxTeamSize",
                table: "Quests",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Quests",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Quests",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Participants",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrincipalUserId",
                table: "Participants",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SoloInfiniteQuestId",
                table: "Participants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Participants",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamScheduledQuestId",
                table: "Participants",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participants",
                table: "Participants",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UsedParticipantHints",
                columns: table => new
                {
                    HintId = table.Column<int>(nullable: false),
                    ParticipantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsedParticipantHints", x => new { x.ParticipantId, x.HintId });
                    table.ForeignKey(
                        name: "FK_UsedParticipantHints_Hints_HintId",
                        column: x => x.HintId,
                        principalTable: "Hints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsedParticipantHints_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskAttempts_ParticipantId",
                table: "TaskAttempts",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_PrincipalUserId",
                table: "Participants",
                column: "PrincipalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_SoloInfiniteQuestId",
                table: "Participants",
                column: "SoloInfiniteQuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_ApplicationUserId",
                table: "Participants",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_PrincipalUserId1",
                table: "Participants",
                column: "PrincipalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_TeamScheduledQuestId",
                table: "Participants",
                column: "TeamScheduledQuestId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedParticipantHints_HintId",
                table: "UsedParticipantHints",
                column: "HintId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_AspNetUsers_ModeratorId",
                table: "Participants",
                column: "ModeratorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Quests_QuestId",
                table: "Participants",
                column: "QuestId",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_AspNetUsers_PrincipalUserId",
                table: "Participants",
                column: "PrincipalUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Quests_SoloInfiniteQuestId",
                table: "Participants",
                column: "SoloInfiniteQuestId",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Quests_TeamScheduledQuestId",
                table: "Participants",
                column: "TeamScheduledQuestId",
                principalTable: "Quests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAttempts_Participants_ParticipantId",
                table: "TaskAttempts",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamUsers_Participants_TeamId",
                table: "TeamUsers",
                column: "TeamId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_AspNetUsers_ModeratorId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Quests_QuestId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_AspNetUsers_PrincipalUserId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Quests_SoloInfiniteQuestId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_AspNetUsers_ApplicationUserId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_AspNetUsers_PrincipalUserId1",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Quests_TeamScheduledQuestId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAttempts_Participants_ParticipantId",
                table: "TaskAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamUsers_Participants_TeamId",
                table: "TeamUsers");

            migrationBuilder.DropTable(
                name: "UsedParticipantHints");

            migrationBuilder.DropIndex(
                name: "IX_TaskAttempts_ParticipantId",
                table: "TaskAttempts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participants",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_PrincipalUserId",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_SoloInfiniteQuestId",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_ApplicationUserId",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_PrincipalUserId1",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_TeamScheduledQuestId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "ParticipantId",
                table: "TaskAttempts");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "PrincipalUserId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "SoloInfiniteQuestId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "TeamScheduledQuestId",
                table: "Participants");

            migrationBuilder.RenameTable(
                name: "Participants",
                newName: "Teams");

            migrationBuilder.RenameIndex(
                name: "IX_Participants_QuestId",
                table: "Teams",
                newName: "IX_Teams_QuestId");

            migrationBuilder.RenameIndex(
                name: "IX_Participants_ModeratorId",
                table: "Teams",
                newName: "IX_Teams_ModeratorId");

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "TaskAttempts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Quests",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ResultsAvailable",
                table: "Quests",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDeadline",
                table: "Quests",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaxTeamSize",
                table: "Quests",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Quests",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CaptainUserId",
                table: "Teams",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teams",
                table: "Teams",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UsedTeamHints",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "integer", nullable: false),
                    HintId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsedTeamHints", x => new { x.TeamId, x.HintId });
                    table.ForeignKey(
                        name: "FK_UsedTeamHints_Hints_HintId",
                        column: x => x.HintId,
                        principalTable: "Hints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsedTeamHints_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskAttempts_TeamId",
                table: "TaskAttempts",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CaptainUserId",
                table: "Teams",
                column: "CaptainUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedTeamHints_HintId",
                table: "UsedTeamHints",
                column: "HintId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAttempts_Teams_TeamId",
                table: "TaskAttempts",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_AspNetUsers_CaptainUserId",
                table: "Teams",
                column: "CaptainUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_AspNetUsers_ModeratorId",
                table: "Teams",
                column: "ModeratorId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_TeamUsers_Teams_TeamId",
                table: "TeamUsers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
