using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quest.DAL.Migrations
{
    public partial class QuestUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Quests");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Quests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InviteTokenSecret",
                table: "Quests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDeadline",
                table: "Quests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Quests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "InviteTokenSecret",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "RegistrationDeadline",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Quests");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Quests",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
