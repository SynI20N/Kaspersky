using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatisticsAPI.Migrations
{
    /// <inheritdoc />
    public partial class Stats1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeSpamp",
                table: "Statistics",
                newName: "TimeStamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "Statistics",
                newName: "TimeSpamp");
        }
    }
}
