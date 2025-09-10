using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TImeScaleDB.Migrations
{
    /// <inheritdoc />
    public partial class AlertFull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    UniqueID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.UniqueID);
                });

            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    UniqueID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmailValue = table.Column<string>(type: "text", nullable: true),
                    NotificationGroupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.UniqueID);
                    table.ForeignKey(
                        name: "FK_Emails_Groups_NotificationGroupId",
                        column: x => x.NotificationGroupId,
                        principalTable: "Groups",
                        principalColumn: "UniqueID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupAlerts",
                columns: table => new
                {
                    UniqueID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AlertRuleId = table.Column<int>(type: "integer", nullable: false),
                    NotificationGroupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAlerts", x => x.UniqueID);
                    table.ForeignKey(
                        name: "FK_GroupAlerts_Groups_NotificationGroupId",
                        column: x => x.NotificationGroupId,
                        principalTable: "Groups",
                        principalColumn: "UniqueID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupAlerts_Rules_AlertRuleId",
                        column: x => x.AlertRuleId,
                        principalTable: "Rules",
                        principalColumn: "UniqueID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Telegrams",
                columns: table => new
                {
                    UniqueID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TelegramChatId = table.Column<int>(type: "integer", nullable: false),
                    NotificationGroupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telegrams", x => x.UniqueID);
                    table.ForeignKey(
                        name: "FK_Telegrams_Groups_NotificationGroupId",
                        column: x => x.NotificationGroupId,
                        principalTable: "Groups",
                        principalColumn: "UniqueID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Emails_NotificationGroupId",
                table: "Emails",
                column: "NotificationGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAlerts_AlertRuleId",
                table: "GroupAlerts",
                column: "AlertRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAlerts_NotificationGroupId",
                table: "GroupAlerts",
                column: "NotificationGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Telegrams_NotificationGroupId",
                table: "Telegrams",
                column: "NotificationGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropTable(
                name: "GroupAlerts");

            migrationBuilder.DropTable(
                name: "Telegrams");

            migrationBuilder.DropTable(
                name: "Groups");
        }
    }
}
