using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Quest.DAL.Migrations
{
    public partial class HintsAndTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575");

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Reward = table.Column<int>(nullable: false),
                    QuestId = table.Column<int>(nullable: false),
                    VerificationType = table.Column<int>(nullable: false),
                    Question = table.Column<string>(nullable: true),
                    CorrectAnswer = table.Column<string>(nullable: true),
                    Group = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaskId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Secret = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hints_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hints_TaskId",
                table: "Hints",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_QuestId",
                table: "Tasks",
                column: "QuestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hints");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AvatarUrl", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a18be9c0-aa65-4af8-bd17-00bd9344e575", 0, null, "0afc2a93-93d5-4558-a8f5-5eef2f8a7ae3", "some-admin-email@email.fake", true, false, null, "some-admin-email@email.fake", "firstuser", "AQAAAAEAACcQAAAAEG5b2zlGXR2dY4IoKSvcncLrC4DrkeXDT31SBbBVJL+jJviGs7k9L/ENbziibN6cdQ==", null, false, "", false, "firstuser" });
        }
    }
}
