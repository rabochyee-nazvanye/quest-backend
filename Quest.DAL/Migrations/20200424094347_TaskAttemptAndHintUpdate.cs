using Microsoft.EntityFrameworkCore.Migrations;

namespace Quest.DAL.Migrations
{
    public partial class TaskAttemptAndHintUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Hints");

            migrationBuilder.AddColumn<int>(
                name: "UsedHintsCount",
                table: "TaskAttempts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsedHintsCount",
                table: "TaskAttempts");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Hints",
                type: "text",
                nullable: true);
        }
    }
}
