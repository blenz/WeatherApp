using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WeatherApp.Migrations
{
    public partial class AddWeather : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weather",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Address = table.Column<string>(nullable: true),
                    CurrentTemp = table.Column<int>(nullable: false),
                    MinTemp = table.Column<int>(nullable: false),
                    MaxTemp = table.Column<int>(nullable: false),
                    Zip = table.Column<int>(nullable: false),
                    Cached = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weather", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weather");
        }
    }
}
