using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Quest.DAL.Migrations
{
    public partial class NewTaskAttempts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskAttempts",
                table: "TaskAttempts");

            migrationBuilder.DropIndex(
                name: "IX_TaskAttempts_TaskId",
                table: "TaskAttempts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TaskAttempts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskAttempts",
                table: "TaskAttempts",
                columns: new[] { "TaskId", "ParticipantId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskAttempts",
                table: "TaskAttempts");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TaskAttempts",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskAttempts",
                table: "TaskAttempts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAttempts_TaskId",
                table: "TaskAttempts",
                column: "TaskId");
        }
    }
}
