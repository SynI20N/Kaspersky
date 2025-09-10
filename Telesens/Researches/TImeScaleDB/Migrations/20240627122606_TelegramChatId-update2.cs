using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TImeScaleDB.Migrations
{
    /// <inheritdoc />
    public partial class TelegramChatIdupdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TelegramChatId",
                table: "Telegrams",
                type: "text",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "TelegramChatId",
                table: "Telegrams",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
