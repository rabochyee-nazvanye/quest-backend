using Microsoft.EntityFrameworkCore.Migrations;

namespace Quest.DAL.Migrations
{
    public partial class AddHiddenQuest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Quests",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Quests");
        }
    }
}
