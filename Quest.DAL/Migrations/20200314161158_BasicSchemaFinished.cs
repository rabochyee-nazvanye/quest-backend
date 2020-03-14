using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Quest.DAL.Migrations
{
    public partial class BasicSchemaFinished : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUserQuests",
                columns: table => new
                {
                    QuestId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "TaskAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaskId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    PhotoUrl = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    AdminComment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskAttempts_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskAttempts_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskAttemptTeams",
                columns: table => new
                {
                    TeamId = table.Column<int>(nullable: false),
                    TaskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAttemptTeams", x => new { x.TeamId, x.TaskId });
                    table.ForeignKey(
                        name: "FK_TaskAttemptTeams_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskAttemptTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsedTeamHints",
                columns: table => new
                {
                    HintId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false)
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
                name: "IX_AppUserQuests_UserId",
                table: "AppUserQuests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAttempts_TaskId",
                table: "TaskAttempts",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAttempts_TeamId",
                table: "TaskAttempts",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAttemptTeams_TaskId",
                table: "TaskAttemptTeams",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedTeamHints_HintId",
                table: "UsedTeamHints",
                column: "HintId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserQuests");

            migrationBuilder.DropTable(
                name: "TaskAttempts");

            migrationBuilder.DropTable(
                name: "TaskAttemptTeams");

            migrationBuilder.DropTable(
                name: "UsedTeamHints");
        }
    }
}
