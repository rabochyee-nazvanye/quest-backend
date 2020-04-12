using Microsoft.EntityFrameworkCore.Migrations;

namespace Quest.DAL.Migrations
{
    public partial class updateTeamsAndQuests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InviteTokenSecret",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxTeamSize",
                table: "Quests",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InviteTokenSecret",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "MaxTeamSize",
                table: "Quests");
        }
    }
}
