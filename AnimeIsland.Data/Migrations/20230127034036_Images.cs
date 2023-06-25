using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimeIsland.Data.Migrations
{
    public partial class Images : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocalImagem",
                table: "Animes",
                type: "nvarchar(1024)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalImagem",
                table: "Animes");
        }
    }
}
