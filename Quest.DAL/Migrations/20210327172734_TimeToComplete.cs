using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quest.DAL.Migrations
{
    public partial class TimeToComplete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeToComplete",
                table: "Quests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TasksOpenTime",
                table: "Participants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeToComplete",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "TasksOpenTime",
                table: "Participants");
        }
    }
}
